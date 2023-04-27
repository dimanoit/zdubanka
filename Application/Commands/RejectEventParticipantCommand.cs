﻿using Application.Interfaces;
using Domain.Models;
using Domain.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record RejectEventParticipantCommand(EventParticipantStateRequest Request) : IRequest<Result<bool>>;

public class RejectEventParticipantCommandHandler : IRequestHandler<RejectEventParticipantCommand, Result<bool>>
{
    private readonly IApplicationDbContext _dbContext;

    public RejectEventParticipantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(RejectEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipant = await _dbContext.AppointmentParticipants
            .FirstAsync(ap => ap.Id == request.Request.EventParticipantId, cancellationToken);

        eventParticipant.UpdateToRejectStatus();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success();
    }
}