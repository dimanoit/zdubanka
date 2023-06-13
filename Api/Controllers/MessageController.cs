using Application.Services.Interfaces;
using Domain.Requests.Chat;
using Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/message")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(
        IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost]
    public async Task SendMessageAsync(
        SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        await _messageService.SendMessageAsync(request, cancellationToken);
    }

    [HttpGet]
    public async Task<GetMessagesResponse> GetMessagesAsync(
        [FromQuery] GetMessagesRequest request,
        CancellationToken cancellationToken)
    {
        return await _messageService.GetMessagesAsync(request, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteMessagesAsync(
        [FromQuery] DeleteMessageRequest request,
        CancellationToken cancellationToken)
    {
        await _messageService.DeleteMessagesAsync(request, cancellationToken);
    }
}