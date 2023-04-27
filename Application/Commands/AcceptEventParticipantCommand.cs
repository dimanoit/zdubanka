using Application.Interfaces;
using Domain.Models;
using Domain.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record AcceptEventParticipantCommand(EventParticipantStateRequest Request) : IRequest<Result<bool>>;

public class AcceptEventParticipantCommandHandler : IRequestHandler<AcceptEventParticipantCommand, Result<bool>>
{
    private readonly IApplicationDbContext _dbContext;

    public AcceptEventParticipantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(AcceptEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipant = await _dbContext.AppointmentParticipants
            .FirstAsync(ap => ap.Id == request.Request.EventParticipantId, cancellationToken);

        eventParticipant.UpdateToAcceptStatus();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success();
    }
}