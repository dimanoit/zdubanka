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
        var result = chat.ToChatDto();

        // Assert
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
            Members = new[] { "Member1", "Member2" },
            Name = "Test Chat"
        };

        // Act
        var result = request.ToChatEntity();

        // Assert
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
            Members = new[] { "Member1", "Member2" },
            Name = "Updated Chat"
        };

        // Act
        var result = request.ToChatEntity();

        // Assert
        result.Should().NotBeNull();
        result.Members.Should().BeEquivalentTo(request.Members);
        result.Name.Should().Be(request.Name);
    }
}
