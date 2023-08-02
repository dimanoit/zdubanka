using Application.Interfaces;
using Application.Validation.Rules;
using ApplicationUnitTests.Fakers;
using Domain.Entities;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace ApplicationUnitTests.Validators.Rules;

public class ChatRulesTests
{
    private readonly IApplicationDbContext _dbContext;

    public ChatRulesTests()
    {
        _dbContext = Substitute.For<IApplicationDbContext>();
    }

    [Fact]
    public async Task GetIsUserMemberOfChat_UserIsMember_ReturnsTrue()
    {
        // Arrange
        var chat = ChatFaker.Create();
        SetupChatsDbSet(new List<Chat> { chat });

        // Act
        var isUserMemberOfChat = await _dbContext.GetIsUserMemberOfChat(chat.Members[0], chat.Id, default);

        // Assert
        isUserMemberOfChat.Should().BeTrue();
    }

    [Fact]
    public async Task GetIsUserAuthorOfMessage_UserIsAuthor_ReturnsTrue()
    {
        // Arrange
        var message = MessageFaker.Create();
        SetupMessagesDbSet(new List<Message> { message });

        // Act 
        var result = await _dbContext.GetIsUserAuthorOfMessage(message.SenderId, message.Id, default);

        // Assert
        result.Should().BeTrue();
    }

    private void SetupChatsDbSet(List<Chat> chats)
    {
        var dbSet = chats.AsQueryable().BuildMockDbSet();
        _dbContext.Chats.Returns(dbSet);
    }

    private void SetupMessagesDbSet(List<Message> messages)
    {
        var dbSet = messages.AsQueryable().BuildMockDbSet();
        _dbContext.Messages.Returns(dbSet);
    }
}
