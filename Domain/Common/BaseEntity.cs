using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace Domain.Common;

public abstract class BaseEntity
{
    private readonly List<INotification> _domainEvents = new();
    public string Id { get; init; } = null!;

    [NotMapped] public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(INotification domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}