using System;

namespace Domain;

public class Comment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Body { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //Relationships

    public required string ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    //* Not necesary to create a collection in user entity because there is not a funcionality to see all of the comments per user
    public required string UserId { get; set; }
    public User User { get; set; } = null!;

}
