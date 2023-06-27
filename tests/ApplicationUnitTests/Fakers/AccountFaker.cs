using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace ApplicationUnitTests.Fakers;

public static class AccountFaker
{
    public static Account Create()
    {
        var faker = new Faker<Account>()
            .RuleFor(a => a.Id, f => f.Random.Guid().ToString())
            .RuleFor(a => a.UserName, f => f.Internet.UserName())
            .RuleFor(a => a.NormalizedUserName, (f, a) => a.UserName!.ToUpper())
            .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.UserName))
            .RuleFor(a => a.NormalizedEmail, (f, a) => a.Email!.ToUpper())
            .RuleFor(a => a.EmailConfirmed, true)
            .RuleFor(a => a.FullName, f => f.Person.FullName)
            .RuleFor(a => a.Bio, f => f.Lorem.Sentence())
            .RuleFor(a => a.ImageUrl, f => f.Internet.Avatar())
            .RuleFor(a => a.Gender, f => f.PickRandom<Gender>())
            .RuleFor(a => a.RelationshipStatus, f => f.PickRandom<RelationshipStatus>())
            .RuleFor(a => a.RefreshToken, f => f.Random.Guid().ToString())
            .RuleFor(a => a.RefreshTokenExpiryTime, f => f.Date.Future())
            .RuleFor(a => a.DateOfBirth, f => f.Date.Between(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-18)))
            .RuleFor(a => a.AuthMethod, f => f.PickRandom<AuthMethod>())
            .Ignore(a => a.UserLanguages)
            .Ignore(a => a.Events)
            .Ignore(a => a.EventParticipations);

        return faker.Generate();
    }
}