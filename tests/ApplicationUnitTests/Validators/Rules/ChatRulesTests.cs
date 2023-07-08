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
    [Fact]
    public async Task GetIsUserMemberOfChat_UserIsMember_ReturnsTrue()
    {
        // Arrange
        var dbContext = Substitute.For<IApplicationDbContext>();
        var chat = ChatFaker.Create();

        var dbSet = new List<Chat> { chat }.AsQueryable().BuildMockDbSet();
        dbContext.Chats.Returns(dbSet);
        
        // Act
        var isUserMemberOfChat = await dbContext.GetIsUserMemberOfChat(chat.Members[0], chat.Id, default);

        // Assert
        isUserMemberOfChat.Should().BeTrue();
    }
}