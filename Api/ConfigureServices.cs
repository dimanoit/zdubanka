
using System.Text;
using System.Text.Json.Serialization;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public static class ConfigureServices
{
    public static readonly string CorsPolicyName = "CorsPolicy";

    public static void AddApiServices(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(environment);
        services.AddScoped<AuthService>();

        // TODO get data from IOptions
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenOptions:Key"] ?? throw new InvalidOperationException()))
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
                var allowedOrigins = "http://localhost:4200"; // TODO do not hard code
                builder.WithOrigins(allowedOrigins);
            }

            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }));
    }
}