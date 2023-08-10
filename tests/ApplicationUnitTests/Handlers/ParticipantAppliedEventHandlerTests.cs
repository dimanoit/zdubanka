using Application.EventHandlers;
using Application.Interfaces;
using ApplicationUnitTests.Fakers;
using ApplicationUnitTests.Helpers;
using Domain.Events;
using Domain.Models;
using NSubstitute;
using Xunit;

namespace ApplicationUnitTests.Handlers;

public class ParticipantAppliedEventHandlerTests
{
    [Fact]
    public async Task Handle_WhenApplied_ShouldSendNotificationEmail()
    {
        // Arrange
        var dbContext = ApplicationDbContextFactory.Create();
        var eventEntity = EventFaker.CreateEvent();
        var account = AccountFaker.Create();
        eventEntity.Organizer = account;
        dbContext.Events.Add(eventEntity);
        await dbContext.SaveChangesAsync();

        var emailService = Substitute.For<IEmailService>();

        var handler = new ParticipantAppliedEventHandler(emailService, dbContext);

        var notification = new ParticipantAppliedEvent(string.Empty, eventEntity.Id);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        await emailService.Received(1).SendEmailAsync(Arg.Any<SendEmailBaseRequest>());

    }
}