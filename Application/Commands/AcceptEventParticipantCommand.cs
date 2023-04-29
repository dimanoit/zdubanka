using Application.Interfaces;
using Domain.Models;
using Domain.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record AcceptEventParticipantCommand(EventParticipantStateRequest Request) : IRequest<Result>;

public class AcceptEventParticipantCommandHandler : IRequestHandler<AcceptEventParticipantCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public AcceptEventParticipantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(AcceptEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipant = await _dbContext.AppointmentParticipants
            .FirstAsync(ap => ap.Id == request.Request.EventParticipantId, cancellationToken);

        eventParticipant.UpdateToAcceptStatus();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}