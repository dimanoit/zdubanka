using Application.Interfaces;
using Application.Mappers;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Requests;
using Domain.Response;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Application.Services;

public class EventService : IEventService
{
    private readonly IApplicationDbContext _context;

    public EventService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Event> CreateAsync(
        EventCreationRequest eventRequest,
        string organizerId,
        CancellationToken cancellationToken)
    {
        var eventEntity = eventRequest.ToEvent(organizerId);
        eventEntity.AddDomainEvent(new EventCreatedEvent(eventEntity.Id));

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync(cancellationToken);
        return eventEntity;
    }

    public async Task ApplyOnEventAsync(
        string eventId,
        string userId,
        CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events
            .Where(ap => ap.Id == eventId)
            .FirstOrDefaultAsync(cancellationToken);

        if (eventEntity == null) throw new NotFoundException();
        if (eventEntity.OrganizerId == userId) throw new ValidationException();
        if (eventEntity.Status != EventStatus.Opened) throw new ValidationException();

        var eventParticipant = new EventParticipant
        {
            UserId = userId,
            EventId = eventId
        };
        var participantAppliedEvent = new ParticipantAppliedEvent(userId, eventId);

        eventParticipant.AddDomainEvent(participantAppliedEvent);

        _context.EventParticipants.Add(eventParticipant);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<EventResponse> GetUsersEventsAsync(
        EventRetrieveRequest request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _context.Events
            .AsNoTracking()
            .Where(ap => ap.OrganizerId == request.UserId)
            .Select(ap => ap.ToEventResponseDto());

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        var data = await baseQuery.Skip(request.Skip).Take(request.Take).ToArrayAsync(cancellationToken);

        return new EventResponse
        {
            Data = data,
            TotalCount = totalCount
        };
    }
}