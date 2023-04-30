using Application.Behaviours;
using Application.Commands;
using Application.Services;
using Application.Services.Interfaces;
using Application.Validation.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        services.AddValidatorsFromAssemblyContaining<AcceptEventParticipantCommandValidator>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AcceptEventParticipantCommand>();
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
    }
}