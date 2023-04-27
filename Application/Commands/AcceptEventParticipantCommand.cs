using Domain.Models;
using Domain.Requests;
using MediatR;

namespace Application.Commands;

public class AcceptEventParticipantCommand : IRequest<Result<bool>>
{
    public AcceptEventParticipantCommand(EventParticipantStateRequest request)
    {
        throw new NotImplementedException();
    }
}