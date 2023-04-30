using Application.Interfaces;
using Domain.Enums;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.EventHandlers;

public class ParticipantAcceptedEventHandler : INotificationHandler<ParticipantAcceptedEvent>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ParticipantAcceptedEventHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    // TODO optimize query 
    public async Task Handle(ParticipantAcceptedEvent notification, CancellationToken cancellationToken)
    {
        var appointment = await _applicationDbContext.Appointments
            .Include(ap => ap.AppointmentParticipants)
            .FirstAsync(ap => ap.Id == notification.AppointmentId, cancellationToken);

        var countOfPeople = appointment.AppointmentLimitation.CountOfPeople;
        var alreadyAcceptedPeople = appointment.AppointmentParticipants!.Count(ap =>
            ap.AppointmentId == notification.AppointmentId &&
            ap.Status == ParticipantStatus.Accepted
        );

        if (countOfPeople != alreadyAcceptedPeople) return;

        appointment.Status = EventStatus.Closed;
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}