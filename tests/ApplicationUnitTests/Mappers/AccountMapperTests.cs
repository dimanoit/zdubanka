using Application.Mappers;
using ApplicationUnitTests.Fakers;
using Domain.Enums;
using Domain.Models;
using Domain.Requests;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Mappers;

public class AccountMapperTests
{
    [Fact]
    public void UpdateAccount_ShouldUpdateAccountProperties()
    {
        // Arrange
        var accountRequest = new UpdateAccountRequest
        {
            FullName = "John Doe",
            Bio = "Test bio",
            ImageUrl = "test.jpg",
            RelationshipStatus = RelationshipStatus.Single,
            UserLanguages = new List<UserLanguage>
            {
                new() { Language = Language.English, Fluency = LanguageFluency.Intermediate },
                new() { Language = Language.Spanish, Fluency = LanguageFluency.Beginner }
            }
        };
        var account = AccountFaker.Create();

        // Act
        accountRequest.UpdateAccount(account);

        // Assert
        account.FullName.Should().Be(accountRequest.FullName);
        account.Bio.Should().Be(accountRequest.Bio);
        account.ImageUrl.Should().Be(accountRequest.ImageUrl);
        account.RelationshipStatus.Should().Be(accountRequest.RelationshipStatus);
        account.UserLanguages.Should().BeEquivalentTo(accountRequest.UserLanguages);
    }
}