using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController, Authorize, Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> AddMessage([FromBody] Message message)
    {
        await _messageService.AddMessageAsync(message);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(string id, CancellationToken cancellationToken = default)
    {
        var message = await _messageService.GetMessageByIdAsync(id, cancellationToken);
        if (message == null) return NotFound();
        return Ok(message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await _messageService.GetAllMessagesAsync();
        return Ok(messages);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMessage([FromBody] Message message)
    {
        await _messageService.UpdateMessageAsync(message);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(string id)
    {
        var message = await _messageService.GetMessageByIdAsync(id);
        if (message == null) return NotFound();

        await _messageService.DeleteMessageAsync(message);
        return Ok();
    }
}