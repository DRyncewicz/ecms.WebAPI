using ecms.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = "1");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            }).AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureDeleted();
            }
        });
    }

    public HttpClient GetClient(bool authenticated = false)
    {
        var client = CreateClient();
        if (authenticated)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        }
        return client;
    }

    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public string DefaultUserId { get; set; } = null!;
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const string UserId = "UserId";

        public const string AuthenticationScheme = "Test";
        private readonly string _defaultUserId;

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _defaultUserId = options.CurrentValue.DefaultUserId;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

            // Extract User ID from the request headers if it exists,
            // otherwise use the default User ID from the options.
            if (Context.Request.Headers.TryGetValue(UserId, out var userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
            }

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}