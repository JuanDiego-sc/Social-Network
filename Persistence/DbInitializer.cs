using System;
using Domain;

namespace Persistence;

public class DbInitializer
{
    public static async Task SeedData (AppDbContext context)
    {
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
