using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.EventHandlers;

public class ParticipantAppliedEventHandler : INotificationHandler<ParticipantAppliedEvent>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IEmailService _emailService;

    public ParticipantAppliedEventHandler(IEmailService emailService, IApplicationDbContext applicationDbContext)
    {
        _emailService = emailService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(ParticipantAppliedEvent notification, CancellationToken cancellationToken)
    {
        var organizerEmail = await _applicationDbContext.Events
            .Include(ap => ap.Organizer)
            .Where(ap => ap.Id == notification.EventId)
            .Select(ap => ap.Organizer!.Email)
            .FirstAsync(cancellationToken);

              var messageBody = $"User {notification.UserId} applied on your event";

        await _emailService.SendEmailAsync(null);
    }
}