using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class EditProfile
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }

    public class Handler(AppDbContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
{
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {

            var user = await userAccessor.GetUserAsync();

            var updateProfile = context.Users.FirstOrDefault(x => x.Id == user.Id);

            if (updateProfile == null) return Result<Unit>.Failure("User not found in DB", 400);

            updateProfile.DisplayName = request.DisplayName;
            updateProfile.Bio = request.Bio;

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result
                   ? Result<Unit>.Success(Unit.Value)
                   : Result<Unit>.Failure("Failed to save en DB", 400);
        }
    }
}
