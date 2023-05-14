using Application.Interfaces;
using Application.Mappers;
using Domain.Requests;
using Domain.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public record EventsQuery(SearchEventRequest Request) : IRequest<EventResponse>;

internal class EventsQueryHandler : IRequestHandler<EventsQuery, EventResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public EventsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EventResponse> Handle(EventsQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        // TODO add location filter
        var dbQuery = _dbContext.Events
            .AsNoTracking();

        if (request.StartDate.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.StartDay >= request.StartDate);
        }

        if (request.EndDate.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.EndDay <= request.EndDate);
        }

        if (!string.IsNullOrEmpty(request.SearchKeyword))
        {
            // TODO think about full text search 
            dbQuery = dbQuery.Where(ap => ap.Title.Contains(request.SearchKeyword) || ap.Description.Contains(request.SearchKeyword));
        }

        if (request.PeopleCount.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.CountOfPeople == request.PeopleCount);
        }

        if (request.Gender.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.Gender.Contains(request.Gender.Value));
        }

        if (request.RelationshipStatus.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.RelationshipStatus.Contains(request.RelationshipStatus.Value));
        }

        if (request.MinAge.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.AgeLimit.Min >= request.MinAge.Value);
        }

        if (request.MaxAge.HasValue)
        {
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.AgeLimit.Max <= request.MaxAge.Value);
        }

        var count = await dbQuery.CountAsync(cancellationToken);
        var data = await dbQuery
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(ev => ev.ToEventResponseDto())
            .ToArrayAsync(cancellationToken);

        return new EventResponse
        {
            Data = data,
            TotalCount = count
        };
    }
}
