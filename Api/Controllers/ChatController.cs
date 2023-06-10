using Application.Services.Interfaces;
using Domain.Models;
using Domain.Requests.Chat;
using Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreateChatRequest request, CancellationToken cancellationToken)
    {
        await _chatService.CreateAsync(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] DeleteChatRequest request, CancellationToken cancellationToken)
    {
        await _chatService.DeleteAsync(request, cancellationToken);
    }

    [HttpPut]
    public async Task UpdateAsync([FromBody] UpdateChatRequest request, CancellationToken cancellationToken)
    {
        await _chatService.UpdateAsync(request, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ChatDto?> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        return await _chatService.GetAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<GetChatResponse> GetAsync([FromQuery] GetChatRequest request,
        CancellationToken cancellationToken)
    {
        return await _chatService.GetAsync(request, cancellationToken);
    }
}