using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Domain.Requests.Chat;

namespace Application.Mappers;

public static class MessageMapper
{
    public static Message ToMessageEntity(
        this SendMessageRequest request,
        ICurrentUserService userService)
    {
        return new Message
        {
            SenderId = userService.UserId!,
            Content = request.Content,
            SentDate = DateTime.Now,
            ChatId = request.ChatId
        };
    }

    public static MessageDto ToMessageDto(this Message message)
    {
        return new MessageDto
        {
            SenderId = message.SenderId,
            Content = message.Content,
            SentDate = message.SentDate,
            ChatId = message.ChatId,
            Id = message.Id
        };
    }
}