using System.Security.Claims;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

//Is necessary to register this class like a service in program.cs because it is an interface  
public class UserAccessor(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    : IUserAccessor
{
    public async Task<User> GetUserAsync()
    {
        return await dbContext.Users.FindAsync(GetUserId())
            ?? throw new UnauthorizedAccessException("No user us logged in");
    }

    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("No user found");
    }

    public async Task<User> GetUserWithPhotoAsync()
    {
        var userId = GetUserId();

        return await dbContext.Users
                    .Include(u => u.Photos)
                    .FirstOrDefaultAsync(u => u.Id == userId)

            ?? throw new UnauthorizedAccessException("No user us logged in");
    }
}
