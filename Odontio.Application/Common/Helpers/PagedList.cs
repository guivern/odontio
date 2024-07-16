using Microsoft.EntityFrameworkCore;

namespace Odontio.Application.Common.Helpers;

public class PagedList<T> : List<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public PagedList()
    {
    }

    public PagedList(int pageNumber, int pageSize, int totalPages, int totalCount, List<T> items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalCount = totalCount;
        this.AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if (pageSize == -1)
            pageSize = await source.CountAsync(cancellationToken);

        var count = await source.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>(pageNumber, pageSize, totalPages, count, items);
    }
}