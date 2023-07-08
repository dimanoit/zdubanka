using Bogus;
using Domain.Entities;

namespace ApplicationUnitTests.Fakers;

public static class MessageFaker
{
    public static Message Create()
    {
        var faker = new Faker<Message>()
            .RuleFor(m => m.Id, f => f.Random.Guid().ToString())
            .RuleFor(m => m.SenderId, f => f.Random.Guid().ToString())
            .RuleFor(m => m.Content, f => f.Lorem.Sentence())
            .RuleFor(m => m.SentDate, f => f.Date.Past())
            .RuleFor(m => m.ChatId, f => f.Random.Guid().ToString())
            .RuleFor(m => m.Chat, f => f.PickRandom<Chat>());

        return faker.Generate();
    }
}