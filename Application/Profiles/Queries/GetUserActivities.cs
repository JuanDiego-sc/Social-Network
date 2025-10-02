using System.ComponentModel.Design.Serialization;
using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Queries;

public class GetUserActivities
{
    public class Query : IRequest<Result<List<UserActivityDto>>>
    {
        public required UserActivityParams Params { get; set; }
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor) 
        : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
        public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = context.Activities
                .OrderBy(a => a.Date)
                .Where(a => a.Attendees.Any(u => u.UserId == request.UserId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Params.Filter))
            {
                query = request.Params.Filter switch
                {
                    "IsHost" => query.Where(x => x.Attendees.Any(a =>
                        a.IsHost && a.UserId == userAccessor.GetUserId())),
                    "Past" => query.Where(x => x.Attendees.Any(a =>
                        a.UserId == userAccessor.GetUserId()
                        && x.Date <= DateTime.UtcNow)),
                    "Future" => query.Where(x => x.Attendees.Any(a =>
                        a.UserId == userAccessor.GetUserId()
                        && x.Date >= DateTime.UtcNow)),
                    _ => query
                };
            }

            var projectedActivities = query
                .ProjectTo<UserActivityDto>(mapper.ConfigurationProvider);
                
            var activities = await projectedActivities 
            .ToListAsync(cancellationToken);

            return Result<List<UserActivityDto>>
            .Success(mapper.Map<List<UserActivityDto>>(activities));
        }
    }
}
