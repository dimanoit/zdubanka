using System.Text;
using System.Text.Json.Serialization;
using Api.Services;
using Api.Swagger;
using Application.Services.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api;

public static class ConfigureServices
{
    public static readonly string CorsPolicyName = "CorsPolicy";

    public static void AddApiServices(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        services.AddCors(environment);
        services.AddScoped<AuthService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["TokenOptions:Audience"],
                    ValidIssuer = configuration["TokenOptions:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["TokenOptions:Key"] ??
                                               throw new InvalidOperationException()))
                };
            });
    }

    private static void AddCors(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
        {
            if (environment.IsDevelopment())
            {
                builder.SetIsOriginAllowed(_ => true);
            }
            else
            {
                var allowedOrigins = "http://localhost:4200";
                builder.WithOrigins(allowedOrigins);
            }

            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }));
    }
}