using Domain.Entities;
using Domain.Models;
using Domain.Requests.Chat;

namespace Application.Mappers;

public static class ChatMapper
{
    public static ChatDto ToChatDto(this Chat chat)
    {
        return new ChatDto
        {
            Id = chat.Id,
            Members = chat.Members,
            Name = chat.Name
        };
    }

    public static Chat ToChatEntity(this CreateChatRequest request)
    {
        return new Chat
        {
            Members = request.Members,
            Created = DateTime.UtcNow,
            Name = request.Name
        };
    }

    public static Chat ToChatEntity(this UpdateChatRequest request)
    {
        return new Chat
        {
            Id = request.ChatId,
            Members = request.Members,
            Name = request.Name
        };
    }
}