using System;

namespace Infrastructure.Photos;

public class CloudinarySettings
{
    //This prop names must to be the same like in AppSettings.json file
    public required string CloudName { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
