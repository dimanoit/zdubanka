using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Validation.Rules;

public static class ChatRules
{
    public static async Task<bool> GetIsUserMemberOfChat(
        this IApplicationDbContext dbContext,
        string userId,
        string chatId,
        CancellationToken cancellationToken)
    {
        var isUserMemberOfChat = await dbContext.Chats.AnyAsync(
            chat => chat.Id == chatId &&
                    chat.Members.Any(id => id == userId)
            , cancellationToken);

        return isUserMemberOfChat;
    }

    public static async Task<bool> GetIsUserAuthorOfMessage(
        this IApplicationDbContext dbContext,
        string userId,
        string messageId,
        CancellationToken cancellationToken)
    {
        var isUserMemberOfChat = await dbContext.Messages.AnyAsync(
            message => message.Id == messageId &&
                       message.SenderId == userId
            , cancellationToken);

        return isUserMemberOfChat;
    }
}