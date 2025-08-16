using Microsoft.AspNetCore.Mvc;

namespace Ambev.Ominia.Application.Common;

public abstract record PaginatedListQuery
{
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "_size")]
    public int PageSize { get; set; } = 10;

    [FromQuery(Name = "_order")]
    public string? OrderBy { get; set; }
}