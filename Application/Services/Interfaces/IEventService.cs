using Domain.Entities;
using Domain.Requests;
using Domain.Response;

namespace Application.Services.Interfaces;

public interface IEventService
{
    Task<Event> CreateAsync(
        EventCreationRequest eventRequest,
        string organizerId,
        CancellationToken cancellationToken);

    public Task<EventResponse> GetUsersEventsAsync(
        EventRetrieveRequest request,
        CancellationToken cancellationToken);

    Task ApplyOnEventAsync(string eventId, string userId, CancellationToken cancellationToken);
}