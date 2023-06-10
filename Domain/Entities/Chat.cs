using Domain.Common;

namespace Domain.Entities;

public class Chat : BaseEntity
{
    public string[] Members { get; init; } = null!;
    public DateTime Created { get; init; }

    public ICollection<Message>? Messages { get; set; }
    public string Name { get; init; } = null!;
}