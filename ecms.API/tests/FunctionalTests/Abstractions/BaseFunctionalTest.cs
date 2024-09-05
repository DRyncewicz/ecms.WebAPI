using ecms.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace FunctionalTests.Abstractions;

public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;

    private readonly Guid TestDbName = Guid.NewGuid();

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        _scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        ApplicationDbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        HttpClient = factory.GetClient(false);
        AuthorizedHttpClient = factory.GetClient(true);
        CreateDb();
    }

    protected ApplicationDbContext ApplicationDbContext { get; }

    protected HttpClient HttpClient { get; }

    protected HttpClient AuthorizedHttpClient { get; }

    private void CreateDb()
    {
        ApplicationDbContext.Database.SetConnectionString(string.Format(TestConnectionString.testConnectionStringTemplate, TestDbName));
        ApplicationDbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        ApplicationDbContext.Database.SetConnectionString(string.Format(TestConnectionString.testConnectionStringTemplate, TestDbName));
        ApplicationDbContext.Database.EnsureDeleted();
        _scope.Dispose();
        GC.SuppressFinalize(this);
    }
}