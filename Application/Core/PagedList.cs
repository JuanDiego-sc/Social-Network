using System;

namespace Application.Core;

public class PagedList<T, TCursor>
{
    public List<T> Items { get; set; } = [];

    //Filter parameter it could be a date or status
    public TCursor? NextCursor { get; set; }
}
