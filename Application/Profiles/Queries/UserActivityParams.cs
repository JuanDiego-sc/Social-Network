using Application.Core;
namespace Application.Profiles.Queries;

public class UserActivityParams : PaginationParams<string>
{
    public string? Filter { get; set; }
}
