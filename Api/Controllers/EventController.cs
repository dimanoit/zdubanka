using Api.Extensions;
using Application.Queries;
using Application.Services.Interfaces;
using Domain.Requests;
using Domain.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/event")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IMediator _mediator;

    public EventController(
        IEventService eventService,
        IMediator mediator)
    {
        _eventService = eventService;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(
        [FromBody]
        EventCreationRequest eventCreationRequest,
        CancellationToken cancellationToken)
    {
        var userId = User.GetId();

        var createdEvent = await _eventService.CreateAsync(eventCreationRequest, userId, cancellationToken);
        return Created("api/Event", createdEvent);
    }

    [HttpGet]
    public async Task<EventResponse> GetEventsAsync(
        [FromQuery] SearchEventRequest request,
        CancellationToken cancellationToken)
    {
        var query = new EventsQuery(request);
        return await _mediator.Send(query, cancellationToken);
    }

    [HttpGet("own")]
    public async Task<EventResponse> GetCurrentUserEvents(
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var userId = User.GetId();

        var eventRetrieveRequest = new EventRetrieveRequest
        {
            UserId = userId,
            Skip = skip,
            Take = take
        };

        return await _eventService.GetUsersEventsAsync(eventRetrieveRequest, cancellationToken);
    }

    [HttpPatch("{eventId}/apply")]
    public async Task ApplyOnEventAsync(string eventId, CancellationToken cancellationToken)
    {
        await _eventService.ApplyOnEventAsync(eventId, User.GetId(), cancellationToken);
    }
}