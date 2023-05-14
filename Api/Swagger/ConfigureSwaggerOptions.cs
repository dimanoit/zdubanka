using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        var openApiSecurityScheme = new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please provide a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JTW",
            Scheme = "Bearer"
        };
        
        var openApiSecurityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };

        options.AddSecurityDefinition(" Bearer", openApiSecurityScheme);
        options.AddSecurityRequirement(openApiSecurityRequirement);
    }
}