using Application.Interfaces;
using Application.Mappers;
using Domain.Models;
using Domain.Requests;
using Domain.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public record EventParticipantQuery(EventParticipantRequest Request) : IRequest<EventParticipantsResponse>;

internal class EventParticipantQueryHandler : IRequestHandler<EventParticipantQuery, EventParticipantsResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public EventParticipantQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EventParticipantsResponse> Handle(
        EventParticipantQuery request,
        CancellationToken cancellationToken)
    {
        var eventParticipantsQuery = _dbContext
            .AppointmentParticipants
            .Where(app => app.AppointmentId == request.Request.EventId)
            .AsNoTracking();

        var totalCount = await eventParticipantsQuery.CountAsync(cancellationToken);
        var data = await eventParticipantsQuery
            .Include(ap => ap.Appointment)
            .Include(ap => ap.Account)
            .Skip(request.Request.Skip)
            .Take(request.Request.Take)
            .Select(ap => ap.ToEventParticipant())
            .ToArrayAsync(cancellationToken);

        var result = new EventParticipantsResponse
        {
            Data = data,
            TotalCount = totalCount
        };

        return result;
    }
}