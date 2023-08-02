using Application.Commands;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Domain.Enums;
using Domain.Requests;
using FluentAssertions;
using Xunit;

namespace ApplicationUnitTests.Commands;

public class AcceptEventParticipantCommandsTests
{
    [Fact]
    public async Task Handle_ShouldUpdateEventParticipantToAcceptStatusAndSaveChanges()
    {
        // Arrange
        var testEvent = EventFaker.CreateEventWithParticipants();

        var dbContext = ApplicationDbContextFactory.Create();
        dbContext.Events.Add(testEvent);
        await dbContext.SaveChangesAsync();

        var request = new AcceptEventParticipantCommand(new EventParticipantStateRequest(testEvent.OrganizerId, testEvent.EventParticipants!.First().Id));

        var handler = new AcceptEventParticipantCommandHandler(dbContext);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedEventParticipant = await dbContext.EventParticipants.FindAsync(testEvent.EventParticipants!.FirstOrDefault()!.Id);
        updatedEventParticipant!.Status.Should().Be(ParticipantStatus.Accepted);
    }
}