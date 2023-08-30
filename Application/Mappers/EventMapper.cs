using Application.Models.Requests.Events;
using Domain.Entities;
using Domain.Response;

namespace Application.Mappers;

public static class EventMapper
{
    public static Event ToEvent(
        this EventCreationRequest request,
        string organizerId,
        string pictureUrl)
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
            OrganizerId = organizerId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            PictureUrl = pictureUrl
        };

        return eventEntity;
    }


    public static EventResponseDto ToEventResponseDto(this Event entity)
    {
        var organizer = entity.Organizer!.ToAccountShort();

        var eventEntity = new EventResponseDto
        {
            Location = entity.Location,
            Title = entity.Title,
            Description = entity.Description,
            StartDay = entity.StartDay,
            EndDay = entity.EndDay,
            EventLimitation = entity.EventLimitation,
            Id = entity.Id,
            Status = entity.Status,
            Organizer = organizer
        };

        return eventEntity;
    }
}