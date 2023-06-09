﻿using Application.Commands;
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
            .OverridePropertyName(r => r.Request.OrganizerId)
            .WithMessage("User isn't organizer of this event");


        RuleFor(c => c.Request)
            .MustAsync(async (r, cancellation) =>
                await IsEventOpened(dbContext, r.EventParticipantId, cancellation))
            .OverridePropertyName(r => r.Request.OrganizerId)
            .WithMessage("Event already fully booked");
    }

    private async Task<bool> IsEventOpened(
        IApplicationDbContext dbContext,
        string eventParticipantId,
        CancellationToken cancellation)
    {
        return await dbContext.EventParticipants.Include(ap => ap.Event)
            .Where(ap => ap.Id == eventParticipantId)
            .Where(ap => ap.Event.Status == EventStatus.Opened)
            .AnyAsync(cancellation);
    }
}