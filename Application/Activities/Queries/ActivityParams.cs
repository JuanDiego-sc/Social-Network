using System;
using Application.Core;

namespace Application.Activities.Queries;

//The tipe of the paginationParams is the cursor type like dates, status, etc
public class ActivityParams : PaginationParams<DateTime?>
{
    public string? Filter { get; set; }
    //by default value it will show items in future
    public DateTime? StartDate { get; set; } = DateTime.UtcNow;
}
