namespace Application.Providers.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow => DateTime.UtcNow;
}