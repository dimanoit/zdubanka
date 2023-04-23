using Api.Extensions;
using Api.Filters;
using Application.Interfaces;
using Application.Queries;
using Domain.Requests;
using Domain.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize, ApiController, Route("api/event-participants")]
public class EventParticipantController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMediator _mediator;

    public EventParticipantController(IMediator mediator, IAccountService accountService)
    {
        _mediator = mediator;
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<EventParticipantsResponse> GetEventParticipantsAsync(
        [FromQuery]EventParticipantRestRequest request,
        CancellationToken cancellationToken)
    {
        var organizerId = await HttpContext.GetCurrentUserIdAsync(_accountService);
        var queryRequestModel = new EventParticipantRequest(request.EventId, organizerId);
        var query = new EventParticipantQuery(queryRequestModel);
        
        return await _mediator.Send(query, cancellationToken);
    }
}