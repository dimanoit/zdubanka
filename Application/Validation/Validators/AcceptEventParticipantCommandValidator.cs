using Application.Commands;
using Application.Interfaces;
using Application.Validation.Rules;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Validation.Validators;

public class AcceptEventParticipantCommandValidator : AbstractValidator<AcceptEventParticipantCommand>
{
    public AcceptEventParticipantCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(c => c.Request)
            .MustAsync(async (r, cancellation) =>
                await dbContext.IsEventBelongsToOrganizerAsync(r.OrganizerId, r.EventParticipantId, cancellation))
            .OverridePropertyName(r => r.Request.OrganizerId);
        
        RuleFor(c => c.Request)
            .MustAsync(async (r, cancellation) => await IsEnoughPlaceInEventAsync(dbContext, r.EventParticipantId, cancellation))
            .OverridePropertyName(r => r.Request.OrganizerId);
    }

    private async Task<bool> IsEnoughPlaceInEventAsync(
        IApplicationDbContext dbContext,
        string eventParticipantId,
        CancellationToken cancellationToken)
    {
        var isEnoughPlaceInPlaceEvent = await dbContext.AppointmentParticipants
            .Include(ap => ap.Appointment)
            .Where(ap => ap.Id == eventParticipantId)
            .Where(ap =>
                !ap.Appointment.AppointmentLimitation.CountOfPeople.Any() ||
                ap.Appointment.AppointmentLimitation.CountOfPeople.Max() <= ap.Appointment.AppointmentParticipants!.Count
                (participant => participant.Status == ParticipantStatus.Accepted)
            ).AnyAsync(cancellationToken);

        return isEnoughPlaceInPlaceEvent;
    }
}