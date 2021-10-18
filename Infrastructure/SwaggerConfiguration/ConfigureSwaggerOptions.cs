using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.SwaggerConfiguration
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) 
            => _provider = provider;
        
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions) 
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"ChatAPI {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                    });
            }

            ConfigureBearerAuthorization(options);
        }

        private static void ConfigureBearerAuthorization(SwaggerGenOptions options)
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                BearerFormat = "Bearer {authToken}",
                Description = "JSON Web Token to access resources. Example: Bearer {token}",
                Type = SecuritySchemeType.ApiKey
            };
            
            options.AddSecurityDefinition(
                "Bearer", securityScheme
            );
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, Id = "Bearer"
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });
        }
    }
}