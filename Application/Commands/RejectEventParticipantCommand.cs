using Application.Interfaces;
using Domain.Models;
using Domain.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record RejectEventParticipantCommand(EventParticipantStateRequest Request) : IRequest;

public class RejectEventParticipantCommandHandler : IRequestHandler<RejectEventParticipantCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public RejectEventParticipantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RejectEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipant = await _dbContext.EventParticipants
            .FirstAsync(ap => ap.Id == request.Request.EventParticipantId, cancellationToken);

        eventParticipant.UpdateToRejectStatus();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}