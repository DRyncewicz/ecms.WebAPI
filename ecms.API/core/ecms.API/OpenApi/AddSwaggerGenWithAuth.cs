using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ecms.API.OpenApi;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://auth.ecms.ovh/realms/emcs/protocol/openid-connect/auth"),
                        Scopes = new Dictionary<string, string>
                         {
                            { "openid", "openid" },
                            { "profile", "profile" }
                         }
                    }
                }
            });

            OpenApiSecurityScheme keycloakSecurityScheme = new()
            {
                Reference = new OpenApiReference
                {
                    Id = "Keycloak",
                    Type = ReferenceType.SecurityScheme,
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer",
            };

            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                 { keycloakSecurityScheme, Array.Empty<string>() },
             });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            o.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}