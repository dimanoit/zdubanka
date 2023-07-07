using System.Collections;
using ApplicationUnitTests.Fakers;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Requests;

namespace ApplicationUnitTests.Fixtures;

public class EventsQueryFixture : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { EventFaker.CreateEvent(), new SearchEventRequest() { } };
        yield return new object[] { GetEventForFullySearchRequest(), GetFullySearchRequest() };
    }

    private static SearchEventRequest GetFullySearchRequest()
    {
        var request = new SearchEventRequest
        {
            Latitude = 90,
            Longitude = 90,
            StartDate = new DateTime(DateTime.UtcNow.Year, 05, 05),
            EndDate = new DateTime(DateTime.UtcNow.Year + 5, 05, 05),
            SearchKeyword = "event",
            PeopleCount = 5,
            Gender = Gender.Female,
            Skip = 0,
            Take = 20,
            MaxAge = 80,
            MinAge = 30,
            RelationshipStatus = RelationshipStatus.Single
        };

        return request;
    }


    private Event GetEventForFullySearchRequest()
    {
        var @event = new Event
        {
            Latitude = 90,
            Longitude = 90,
            Description = "event",
            Id = Guid.NewGuid().ToString(),
            StartDay = new DateTime(DateTime.UtcNow.Year, 06, 06),
            EndDay = new DateTime(DateTime.UtcNow.Year, 07, 07),
            Location = new Address(),
            OrganizerId = Guid.NewGuid().ToString(),
            Title = "event",
            EventLimitation = new EventLimitation
            {
                Gender = new[] { Gender.Female },
                RelationshipStatus = new[] { RelationshipStatus.Single },
                CountOfPeople = 5,
                AgeLimit = new AgeLimit
                {
                    Min = 40,
                    Max = 60
                }
            }
        };

        return @event;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}