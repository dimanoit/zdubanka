using System.Collections;
using ApplicationUnitTests.Fakers;
using Domain.Entities;
using Domain.Enums;
using Domain.Requests;

namespace ApplicationUnitTests.Fixtures;

public class EventsQueryFixture : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { EventFaker.CreateEvent(), new SearchEventRequest() { } };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}