using Application.Interfaces;
using Application.Services;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.EventHandlers;

public class ParticipantAppliedEventHandler: INotificationHandler<ParticipantAppliedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IApplicationDbContext _applicationDbContext;

    public ParticipantAppliedEventHandler(IEmailService emailService, IApplicationDbContext applicationDbContext)
    {
        _emailService = emailService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(ParticipantAppliedEvent notification, CancellationToken cancellationToken)
    {
        var organizerEmail = await _applicationDbContext.Appointments
            .Include(ap => ap.Organizer)
            .Where(ap => ap.Id == notification.AppointmentId)
            .Select(ap => ap.Organizer!.Email)
            .FirstAsync(cancellationToken);

        var messageBody = $"User {notification.UserId} applied on your event";

        await _emailService.SendEmailAsync(organizerEmail, messageBody, "Event application");
    }
}

