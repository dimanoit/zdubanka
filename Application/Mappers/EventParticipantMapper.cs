using Domain.Entities;
using Domain.Models;

namespace Application.Mappers;

public static class EventParticipantMapper
{
    public static EventParticipantDto ToEventParticipant(this EventParticipant eventParticipant)
    {
        var participantDto = new EventParticipantDto
        {
            Id = eventParticipant.Id,
            UserId = eventParticipant.UserId,
            UserName = eventParticipant.Account.FullName,
            EventId = eventParticipant.EventId,
            EventTitle = eventParticipant.Event.Title,
            Status = eventParticipant.Status
        };

        return participantDto;
    }
}
