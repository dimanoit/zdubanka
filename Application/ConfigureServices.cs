using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Application.Behaviours;
using Application.Commands;
using Application.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Application.Validation.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;


namespace Application;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("uk-UA"),
                new CultureInfo("pl-PL")
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IMessageService, MessageService>();

        services.AddValidatorsFromAssemblyContaining<AcceptEventParticipantCommandValidator>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AcceptEventParticipantCommand>();
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
    }
}