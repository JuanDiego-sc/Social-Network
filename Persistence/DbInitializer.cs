using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence;

public class DbInitializer
{
    public static async Task SeedData (AppDbContext context, UserManager<User> userManager)
    {

        if (userManager.Users.Any())
        {
            return;
        }

        var users = new List<User>
        {
            new()
            {
                DisplayName = "Jotade",
                UserName = "Jotade@ppm.com",
                Email = "Jotade@ppm.com",
            },
             new()
            {
                DisplayName = "Rene",
                UserName = "Rene@ppm.com",
                Email = "Rene@ppm.com",
            },
             new()
            {
                DisplayName = "Sebas",
                UserName = "Sebas@ppm.com",
                Email = "Sebas@ppm.com",
            },
             new()
            {
                DisplayName = "Dani",
                UserName = "Dani@ppm.com",
                Email = "Dani@ppm.com",
            }
        };

        foreach (var user in users)
        {
            // user the userManager function to create de list of users by Identity framework
            //! save changes is not necessary 
            await userManager.CreateAsync(user, "Pes@j2022*");
        }


        if (context.Activities.Any())
        {
            return; // DB has been seeded
        }

        var activities = new List<Activity> {
            new() {
                Title = "Past Activity 1",
                Date = DateTime.UtcNow.AddMonths(-2),
                Description = "Activity 2 months ago",
                Category = "drinks",
                City = "London",
                Venue = "Pub",
                Latitude = 51.5074,
                Longitude = -0.1278,
            },
            new() {
                Title = "Past Activity 2",
                Date = DateTime.UtcNow.AddMonths(-1),
                Description = "Activity 1 month ago",
                Category = "culture",
                City = "London",
                Venue = "British Museum",
                Latitude = 51.5194,
                Longitude = -0.1270,
            },
            new() {
                Title = "Future Activity 1",
                Date = DateTime.UtcNow.AddMonths(1),
                Description = "Activity in 1 month",
                Category = "culture",
                City = "London",
                Venue = "British Museum",
                Latitude = 51.5194,
                Longitude = -0.1270,
            },
            new() {
                Title = "Future Activity 2",
                Date = DateTime.UtcNow.AddMonths(2),
                Description = "Activity in 2 months",
                Category = "culture",
                City = "London",
                Venue = "British Museum",
                Latitude = 51.5194,
                Longitude = -0.1270,
            },
        };

        context.Activities.AddRange(activities);
        await context.SaveChangesAsync();
    }
}
