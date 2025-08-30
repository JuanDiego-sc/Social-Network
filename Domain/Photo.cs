using System;
using System.Text.Json.Serialization;

namespace Domain;

public class Photo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Url { get; set; }
    public required string PublicId { get; set; }

    //Relationships
    public required string UserId { get; set; }

    [JsonIgnore] //ignore the entity and prevent cycles
    public User User { get; set; } = null!;
}
