
namespace Api;

public static class ConfigureServices
{
    public static readonly string CorsPolicyName = "CorsPolicy";

    public static void AddApiServices(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.Configure<AppSettings>(configuration.GetSection("ApplicationSettings"));
        AddCors(services, environment);
        services.AddScoped<JwtService>();
    }

    private static void AddCors(IServiceCollection services, IWebHostEnvironment environment)
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