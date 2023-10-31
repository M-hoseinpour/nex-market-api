using market.Models.DTO.BaseDto;
using Microsoft.EntityFrameworkCore;

namespace market.Extensions;

public static class QueryableExtensions
{
    public static async Task<FilteredResult<T>> ExecuteWithPaginationAsync<T>(
        this IQueryable<T> query,
        IPaginationQueryParams paginationQueryParams,
        CancellationToken cancellationToken
    )
    {
        var total = query.Count();

        query = query
            .Skip((paginationQueryParams.PageIndex - 1) * paginationQueryParams.PageSize)
            .Take(paginationQueryParams.PageSize);

        var data = await query.ToListAsync(cancellationToken: cancellationToken);

        var result = new FilteredResult<T> { Total = total, Data = data };

        return result;
    }
}