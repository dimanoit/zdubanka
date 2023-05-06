using Application.Interfaces;
using Domain.Models;
using Domain.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record AcceptEventParticipantCommand(EventParticipantStateRequest Request) : IRequest;

public class AcceptEventParticipantCommandHandler : IRequestHandler<AcceptEventParticipantCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public AcceptEventParticipantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AcceptEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipant = await _dbContext.AppointmentParticipants
            .FirstAsync(ap => ap.Id == request.Request.EventParticipantId, cancellationToken);

        eventParticipant.UpdateToAcceptStatus();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}