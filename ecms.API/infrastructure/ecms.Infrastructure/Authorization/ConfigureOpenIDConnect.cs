using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace ecms.Infrastructure.Authorization;

public static class ConfigureOpenIDConnect
{
    public static IServiceCollection AddAuthBearer(this IServiceCollection services)
    {
        AddJWTAuthentication(services);
        services.AddAuthorizationBuilder()
                .AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("profile", "openid");
                });

        return services;
    }

    private static void AddJWTAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
           .AddJwtBearer(options =>
           {
               options.Authority = "https://auth.ecms.ovh/realms/emcs";
               options.Audience = "account";
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidIssuer = "https://auth.ecms.ovh/realms/emcs",
                   ValidateAudience = true,
                   ValidAudience = "account",
                   ValidateLifetime = true
               };
           });
    }
}