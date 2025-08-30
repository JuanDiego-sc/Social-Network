using System;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.Profiles.Commands;

public class SetMainPhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handler(AppDbContext dbContext, IUserAccessor userAccessor) 
        : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserWithPhotoAsync();

            var photo = user.Photos.FirstOrDefault(p => p.Id == request.PhotoId);
            if (photo == null) return Result<Unit>.Failure("Failed to find the photo", 400);

            user.ImageUrl = photo.Url;

            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
            
            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem updating photo", 400);

        }
    }
}
