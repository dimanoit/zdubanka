using Application.Providers.Interfaces;

namespace Application.Providers;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}