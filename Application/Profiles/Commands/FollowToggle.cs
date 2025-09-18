using System;
using System.Security.Cryptography.X509Certificates;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles.Commands;

public class FollowToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor)
        : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await userAccessor.GetUserAsync();
            var target = await context.Users.FindAsync([request.UserId], cancellationToken);

            if (target == null) return Result<Unit>.Failure("Target User not found", 404);

            var followingRequest = await context.UserFollowings
                .FindAsync([observer.Id, target.Id], cancellationToken);

            if (followingRequest == null) context.UserFollowings.Add(
                new UserFollowing
                {
                    ObserverId = observer.Id,
                    TargetId = target.Id
                });
            else context.UserFollowings.Remove(followingRequest);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;
            
             return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem saving the DB", 400);
        }
    }
}
