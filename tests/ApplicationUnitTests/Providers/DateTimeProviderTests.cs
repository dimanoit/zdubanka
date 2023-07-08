using Application.Providers;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Providers;

public class DateTimeProviderTests
{
    [Fact]
    public void UtcNow_ShouldReturnCurrentUtcDateTime()
    {
        // Arrange
        var dateTimeProvider = new DateTimeProvider();

        // Act
        var result = dateTimeProvider.UtcNow;

        // Assert
        result.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}