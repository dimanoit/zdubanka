using System.Collections;
using ApplicationUnitTests.Fakers;
using Domain.Entities;
using Domain.Enums;

namespace ApplicationUnitTests.Fixtures;

public class EventParticipantsFixture : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { GetEvent(), EventStatus.Closed };
        yield return new object[] { EventFaker.CreateEvent(3), EventStatus.Opened };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private Event GetEvent()
    {
        var eventEntity = EventFaker.CreateEvent(3);
        eventEntity.EventParticipants = EventFaker.GenerateFakeEventParticipants((eventEntity.Id, ParticipantStatus.Accepted), 3);

        return eventEntity;
    }
}