using Application.Behaviours;
using Application.Queries;
using ApplicationUnitTests.Helpers;
using Domain.Requests;
using Domain.Response;
using FluentAssertions;
using FluentValidation;
using MediatR;
using NSubstitute;
using Xunit;
using ValidationException = Domain.Exceptions.ValidationException;

namespace ApplicationUnitTests.Behaviours;

public class ValidationBehaviourTests
{
    [Fact]
    public async Task Handle_NoValidators_ReturnsResultFromNext()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<string>>();
        var behavior = new ValidationBehaviour<string, int>(validators);

        var request = "test";
        var expectedResult = 42;
        var next = Substitute.For<RequestHandlerDelegate<int>>();
        next().Returns(expectedResult);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResult);
        await next.Received(1).Invoke();
    }


    [Fact]
    public async Task Handle_EventParticipantQueryValidator_ReturnsValidationErrros()
    {
        // Arrange
        var dbContext = ApplicationDbContextFactory.Create();
        var validators = new[] { new EventParticipantQueryValidator(dbContext) };
        var behavior = new ValidationBehaviour<EventParticipantQuery, EventParticipantsResponse>(validators);

        var request = new EventParticipantQuery(new EventParticipantRequest(default, default));

        // Act
        Func<Task> act = () => behavior.Handle(request, default, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}