using Domain.Models;
using Domain.Requests;
using MediatR;

namespace Application.Commands;

public class RejectEventParticipantCommand: IRequest<Result<bool>>
{
    public RejectEventParticipantCommand(EventParticipantStateRequest request)
    {
        throw new NotImplementedException();
    }
}