using Application.Interfaces;
using Domain.Enums;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.EventHandlers.ParticipantApplied;

public class ParticipantAppliedChangeAppointmentStatus: INotificationHandler<ParticipantAppliedEvent>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ParticipantAppliedChangeAppointmentStatus(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(ParticipantAppliedEvent notification, CancellationToken cancellationToken)
    {
        var appointment = _applicationDbContext.Appointments
            .First(ap => ap.Id == notification.AppointmentId);

        var countOfPeople = appointment.AppointmentLimitation.CountOfPeople;
        var alreadyAcceptedPeople = appointment.AppointmentParticipants
            .Count(ap => ap.AppointmentId == notification.AppointmentId && ap.Status == ParticipantStatus.Accepted);

        if (countOfPeople == alreadyAcceptedPeople)
        {
            appointment.Status = ParticipantStatus.Accepted;
        }

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

    }
}