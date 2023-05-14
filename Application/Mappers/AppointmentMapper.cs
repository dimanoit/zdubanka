using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Application.Mappers;

public static class EventMapper
{
    public static Event ToEvent(this EventCreationRequest request, string organizerId)
    {
        var eventEntity = new Event
        {
            Id = Guid.NewGuid().ToString(),
            Location = request.Location,
            Title = request.Title,
            Description = request.Description,
            StartDay = request.StartDay,
            EndDay = request.EndDay,
            EventLimitation = request.EventLimitation,
            OrganizerId = organizerId
        };

        return eventEntity;
    }


    public static EventResponseDto ToEventResponseDto(this Event entity)
    {
        var eventEntity = new EventResponseDto
        {
            Location = entity.Location,
            Title = entity.Title,
            Description = entity.Description,
            StartDay = entity.StartDay,
            EndDay = entity.EndDay,
            EventLimitation = entity.EventLimitation,
            OrganizerId = entity.OrganizerId,
            Id = entity.Id,
            Status = entity.Status
        };

        return eventEntity;
    }
}