using Api.Extensions;
using Application.Commands;
using Application.Queries;
using Domain.Models;
using Domain.Requests;
using Domain.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize, ApiController, Route("api/event-participants")]
public class EventParticipantController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventParticipantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<EventParticipantsResponse> GetEventParticipantsAsync(
        [FromQuery]
        EventParticipantRestRequest request,
        CancellationToken cancellationToken)
    {
        var queryRequestModel = new EventParticipantRequest(request.EventId, User.GetId());
        var query = new EventParticipantQuery(queryRequestModel);

        return await _mediator.Send(query, cancellationToken);
    }

    [HttpPatch]
    public async Task<Result<bool>> AcceptEventParticipantAsync(
        string eventParticipantId,
        CancellationToken cancellationToken)
    {
        var request = new EventParticipantStateRequest(User.GetId(), eventParticipantId);
        return await _mediator.Send(new AcceptEventParticipantCommand(request), cancellationToken);
    }

    [HttpPatch]
    public async Task<Result<bool>> RejectEventParticipantAsync(
        string eventParticipantId,
        CancellationToken cancellationToken)
    {
        var request = new EventParticipantStateRequest(User.GetId(), eventParticipantId);
        return await _mediator.Send(new RejectEventParticipantCommand(request), cancellationToken);
    }
}