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

    //! just use JsonIgnore if the relation info is not important, instead use DTOs and map the properties
    [JsonIgnore] //ignore the entity and prevent cycles
    public User User { get; set; } = null!;
}
