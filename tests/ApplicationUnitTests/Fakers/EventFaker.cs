using Application.Models.Requests.Events;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace ApplicationUnitTests.Fakers;

public static class EventFaker
{
    private static Address GenerateAddress(Faker faker)
    {
        return new Address
        {
            Street = faker.Address.StreetAddress(),
            City = faker.Address.City(),
            State = faker.Address.State(),
            Country = faker.Address.Country()
        };
    }

    public static EventParticipant[] GenerateFakeEventParticipants(
        (string EventId, ParticipantStatus Status) eventData, int count)
    {
        var result = new EventParticipant[count];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = GenerateFakeEventParticipant(eventData.EventId, eventData.Status);
        }

        return result;
    }

    private static EventParticipant GenerateFakeEventParticipant(
        string eventId,
        ParticipantStatus status)
    {
        var faker = new Faker<EventParticipant>()
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.EventId, f => eventId)
            .RuleFor(p => p.Status, f => status);

        return faker.Generate();
    }

    private static EventLimitation GenerateEventLimitation(Faker faker, int countOfPeople)
    {
        return new EventLimitation
        {
            CountOfPeople = countOfPeople,
            Gender = faker.PickRandom<Gender[]>(faker.Random.EnumValues(exclude: Gender.Other)),
            RelationshipStatus = faker.PickRandom<RelationshipStatus[]>(faker.Random.EnumValues<RelationshipStatus>()),
            AgeLimit = new AgeLimit
            {
                Min = faker.Random.Int(18, 30),
                Max = faker.Random.Int(31, 50)
            }
        };
    }

    public static Event CreateEvent(int countOfPeople = 3)
    {
        var faker = new Faker<Event>()
            .RuleFor(e => e.Id, f => f.Random.Guid().ToString())
            .RuleFor(e => e.Location, GenerateAddress)
            .RuleFor(e => e.Title, f => f.Commerce.ProductName())
            .RuleFor(e => e.Description, f => f.Lorem.Sentence())
            .RuleFor(e => e.StartDay, f => f.Date.Future())
            .RuleFor(e => e.EndDay, (f, e) => f.Date.Between(e.StartDay, e.StartDay.AddDays(7)))
            .RuleFor(e => e.EventLimitation, f => GenerateEventLimitation(f, countOfPeople))
            .RuleFor(e => e.Latitude, f => f.Address.Latitude())
            .RuleFor(e => e.Longitude, f => f.Address.Longitude())
            .RuleFor(e => e.OrganizerId, f => f.Random.Guid().ToString());

        return faker.Generate();
    }

    public static Event CreateEventWithParticipants()
    {
        var account = AccountFaker.Create();
        var eventParticipant = AccountFaker.Create();

        var @event = CreateEvent();
        @event.EventParticipants = new List<EventParticipant>
        {
            new()
            {
                EventId = @event.Id,
                Account = eventParticipant
            }
        };

        @event.Organizer = account;

        return @event;
    }

    public static EventCreationRequest CreateEventCreationRequest()
    {
        var faker = new Faker<EventCreationRequest>()
            .RuleFor(r => r.Location, GenerateAddress)
            .RuleFor(r => r.Latitude, f => f.Address.Latitude())
            .RuleFor(r => r.Longitude, f => f.Address.Longitude())
            .RuleFor(r => r.Title, f => f.Commerce.ProductName())
            .RuleFor(r => r.Description, f => f.Lorem.Sentence())
            .RuleFor(r => r.StartDay, f => f.Date.Future())
            .RuleFor(r => r.EndDay, (f, r) => f.Date.Between(r.StartDay, r.StartDay.AddDays(7)))
            .RuleFor(r => r.EventLimitation, f => GenerateEventLimitation(f, 2));

        return faker.Generate();
    }
}