using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IMessageService, MessageService>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IAccountService>());
    }
}