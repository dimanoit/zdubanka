using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Domain.Requests;
using Domain.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public record EventsQuery(SearchEventRequest Request) : IRequest<EventResponse>;

public class EventsQueryHandler : IRequestHandler<EventsQuery, EventResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public EventsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EventResponse> Handle(EventsQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        var dbQuery = _dbContext.Events
            .AsNoTracking();

        if (request is { DistanceFromKm: not null, Longitude: not null, Latitude: not null })
        {
            dbQuery = GetEventsDistanceFrom(request.Latitude.Value, request.Longitude.Value, request.DistanceFromKm.Value);
        }

        if (request.StartDate.HasValue) dbQuery = dbQuery.Where(ap => ap.StartDay >= request.StartDate);

        if (request.EndDate.HasValue) dbQuery = dbQuery.Where(ap => ap.EndDay <= request.EndDate);

        if (!string.IsNullOrEmpty(request.SearchKeyword))
            dbQuery = dbQuery.Where(ap =>
                ap.Title.Contains(request.SearchKeyword) || ap.Description.Contains(request.SearchKeyword));

        if (request.PeopleCount.HasValue)
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.CountOfPeople == request.PeopleCount);

        if (request.Gender.HasValue)
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.Gender.Contains(request.Gender.Value));

        if (request.RelationshipStatus.HasValue)
            dbQuery = dbQuery.Where(ap =>
                ap.EventLimitation.RelationshipStatus.Contains(request.RelationshipStatus.Value));

        if (request.MinAge.HasValue)
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.AgeLimit.Min >= request.MinAge.Value);

        if (request.MaxAge.HasValue)
            dbQuery = dbQuery.Where(ap => ap.EventLimitation.AgeLimit.Max <= request.MaxAge.Value);

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

    private IQueryable<Event> GetEventsDistanceFrom(
        double myLatitude,
        double myLongitude,
        double distanceThreshold)
    {
        return _dbContext.Events
            .FromSqlInterpolated($@"SELECT * FROM ""Events"" WHERE
             ACOS(SIN(RADIANS({myLatitude})) * SIN(RADIANS(""Latitude"")) +
             COS(RADIANS({myLatitude})) * COS(RADIANS(""Latitude"")) *
             COS(RADIANS(""Longitude"") - RADIANS({myLongitude}))) * 6371 <= {distanceThreshold}");
    }
}