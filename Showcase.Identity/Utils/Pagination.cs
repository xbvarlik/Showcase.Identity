using Microsoft.EntityFrameworkCore;
using Showcase.Identity.Data.Models;

namespace Showcase.Identity.Utils;

public static class Pagination
{
    public static async Task<List<TEntity>> ToPaginatedListAsync<TEntity, TQueryFilterModel>(this IQueryable<TEntity> queryable, TQueryFilterModel query, CancellationToken cancellationToken = default)
        where TEntity : class
        where TQueryFilterModel : BaseQueryFilterModel
    {
        if (query is { PageNumber: not null, PageSize: not null })
            return await queryable.Skip(((int)query.PageNumber! - 1) * (int)query.PageSize!).Take((int)query.PageSize).ToListAsync(cancellationToken);

        return await queryable.ToListAsync(cancellationToken);
    }
    
    public static async Task<List<TEntity>> ToPaginatedListAsync<TEntity, TQueryFilterModel>(this List<TEntity> queryable, TQueryFilterModel query, CancellationToken cancellationToken = default)
        where TEntity : class
        where TQueryFilterModel : BaseQueryFilterModel
    {
        if (query is { PageNumber: not null, PageSize: not null })
            return queryable.Skip(((int)query.PageNumber! - 1) * (int)query.PageSize!).Take((int)query.PageSize).ToList();

        return queryable.ToList();
    }
    
    public static async Task<(List<TEntity>, PaginationModel)> ToPaginatedResultAsync<TEntity, TQueryFilterModel>(
        this IQueryable<TEntity> queryable, 
        TQueryFilterModel query,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TQueryFilterModel : BaseQueryFilterModel
    {
        var count = await queryable.CountAsync(cancellationToken: cancellationToken);
        
        return (await queryable.ToPaginatedListAsync(query, cancellationToken), new PaginationModel
        {
            PageNumber = (query.PageNumber ?? 1),
            PageSize = query.PageSize ?? count,
            TotalPageCount = query.PageSize != null ? (int)Math.Ceiling(count / (double)query.PageSize!) : 1,
        });
    }
    
    public static async Task<(List<TEntity>, PaginationModel)> ToPaginatedResultAsync<TEntity, TQueryFilterModel>(
        this List<TEntity> queryable, 
        TQueryFilterModel query,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TQueryFilterModel : BaseQueryFilterModel
    {
        
        var count = queryable.Count;
        
        return (await queryable.ToPaginatedListAsync(query, cancellationToken), new PaginationModel
        {
            PageNumber = (query.PageNumber ?? 1),
            PageSize = query.PageSize ?? count,
            TotalPageCount = query.PageSize != null ? (int)Math.Ceiling(count / (double)query.PageSize!) : 1,
        });
    }
}