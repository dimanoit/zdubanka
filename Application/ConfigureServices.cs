using System.Reflection;
using Application.Services;
using Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IAccountService>());
    }
}