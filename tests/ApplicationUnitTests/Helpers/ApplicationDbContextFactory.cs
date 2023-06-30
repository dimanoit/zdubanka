using Application.Interfaces;
using ApplicationUnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ApplicationUnitTests.Helpers;

public static class ApplicationDbContextFactory
{
    public static IApplicationDbContext Create()
    {
        var selfOption = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase($"Application DB Context v{Guid.NewGuid()}")
            .EnableSensitiveDataLogging()
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new TestDbContext(selfOption);
    }
}