using Bogus;
using Domain.Entities;

namespace ApplicationUnitTests.Fakers;

public static class ChatFaker
{
    public static Chat Create()
    {
        var chatFaker = new Faker<Chat>()
            .RuleFor(c => c.Id, f => f.Random.Guid().ToString())
            .RuleFor(c => c.Members, f => f.Random.WordsArray(3))
            .RuleFor(c => c.Created, f => f.Date.Past())
            .RuleFor(c => c.Name, f => f.Company.CompanyName());

        return chatFaker.Generate();
    }
}