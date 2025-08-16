using Microsoft.EntityFrameworkCore;

namespace Ambev.Ominia.Application.Common;

public class PaginatedList<T> : List<T>
{
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;

    private int CurrentPage { get; set; }
    private int TotalPages { get; set; }

    private PaginatedList(List<T> items, int totalCount, int currentPage, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, currentPage, pageSize);
    }
}