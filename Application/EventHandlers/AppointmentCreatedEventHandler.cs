using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.EventHandlers;

public class AppointmentCreatedEventHandler : INotificationHandler<AppointmentCreatedEvent>
{
    private readonly IApplicationDbContext _context;

    public AppointmentCreatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task Handle(AppointmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        var chat = new Chat
        {
            CreationDate = DateTime.UtcNow,
            AppointmentId = notification.AppointmentId
        };

        _context.Chats.Add(chat);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
