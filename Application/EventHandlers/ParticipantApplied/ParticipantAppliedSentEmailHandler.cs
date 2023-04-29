using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.EventHandlers.ParticipantApplied;

public class ParticipantAppliedSentEmailHandler: INotificationHandler<ParticipantAppliedEvent>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IEmailService _emailService;

    public ParticipantAppliedSentEmailHandler(IEmailService emailService, IApplicationDbContext applicationDbContext)
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

