using System.Security.Claims;
using Api.Extensions;
using Application.Services.Interfaces;
using Domain.Requests;
using Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(
        [FromBody] EventCreationRequest eventCreationRequest,
        CancellationToken cancellationToken)
    {
        var userId = User.GetId();

        var createdEvent = await _eventService.CreateAsync(eventCreationRequest, userId, cancellationToken);
        return Created("api/Event", createdEvent);
    }

    [HttpGet]
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