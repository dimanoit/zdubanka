using Application.Mappers;
using Application.Services.Interfaces;
using ApplicationUnitTests.Fakers;
using Domain.Entities;
using Domain.Models;
using Domain.Requests.Chat;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ApplicationUnitTests.Mappers;

public class MessageMapperTests
{
    [Fact]
    public void ToMessageEntity_ShouldMapSendMessageRequestToMessageEntity()
    {
        // Arrange
        var request = new SendMessageRequest
        {
            Content = "Test Content",
            ChatId = "TestChatId"
        };
        var userService = Substitute.For<ICurrentUserService>();
        userService.UserId.Returns("TestUserId");
        
        // Act
        var result = request.ToMessageEntity(userService);

        // Assert
        result.SenderId.Should().Be(userService.UserId);
        result.Content.Should().Be(request.Content);
        result.SentDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.ChatId.Should().Be(request.ChatId);
    }

    [Fact]
    public void ToMessageDto_ShouldMapMessageToMessageDto()
    {
        // Arrange
        var message = MessageFaker.Create();

        // Act
        var result = message.ToMessageDto();

        // Assert
        AssertMessageData(result, message);
    }

    private static void AssertMessageData(MessageDto result, Message message)
    {
        result.SenderId.Should().Be(message.SenderId);
        result.Content.Should().Be(message.Content);
        result.SentDate.Should().Be(message.SentDate);
        result.ChatId.Should().Be(message.ChatId);
        result.Id.Should().Be(message.Id);
    }
}