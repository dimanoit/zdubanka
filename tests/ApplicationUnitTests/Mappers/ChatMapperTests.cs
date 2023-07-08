using Application.Mappers;
using ApplicationUnitTests.Fakers;
using Domain.Requests.Chat;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Mappers;

public class ChatMapperTests
{
    [Fact]
    public void ToChatDto_ShouldMapChatToChatDto()
    {
        // Arrange
        var chat = ChatFaker.Create();

        // Act
        var result = ChatMapper.ToChatDto(chat);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(chat.Id);
        result.Members.Should().BeEquivalentTo(chat.Members);
        result.Name.Should().Be(chat.Name);
    }

    [Fact]
    public void ToChatEntity_FromCreateChatRequest_ShouldMapCreateChatRequestToChatEntity()
    {
        // Arrange
        var request = new CreateChatRequest
        {
            Members = new string[] { "Member1", "Member2" },
            Name = "Test Chat"
        };

        // Act
        var result = ChatMapper.ToChatEntity(request);

        // Assert
        result.Should().NotBeNull();
        result.Members.Should().BeEquivalentTo(request.Members);
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public void ToChatEntity_FromUpdateChatRequest_ShouldMapUpdateChatRequestToChatEntity()
    {
        // Arrange
        var request = new UpdateChatRequest
        {
            ChatId = "1",
            Members = new string[] { "Member1", "Member2" },
            Name = "Updated Chat"
        };

        // Act
        var result = ChatMapper.ToChatEntity(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(request.ChatId);
        result.Members.Should().BeEquivalentTo(request.Members);
        result.Name.Should().Be(request.Name);
    }
}