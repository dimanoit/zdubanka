namespace Domain.Entities;

public class Chat
{
    public string AppointmentId { get; init; } = null!;
    public DateTime CreationDate { get; init; }
    public string? ImageUrl { get; init; }

    public ICollection<Message>? Messages { get; set; }
    public Appointment Appointment { get; set; } = null!;
}