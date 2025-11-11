using Application.Common.Mappings;
using Application.Dependencies.DataAccess.Repositories.General;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.General;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories.General;

internal abstract class GuidRepositoryBase<TEntity> : IGuidRepository<TEntity> where TEntity : EntityBase
{
    protected readonly CustomersDbContext _context;
    protected readonly DbSet<TEntity> _set;
    protected readonly IMapper _mapper;

    protected abstract IQueryable<TEntity> BaseQuery { get; }

    protected GuidRepositoryBase(CustomersDbContext context, IMapper mapper)
    {
        _context = context;
        _set = context.Set<TEntity>();
        _mapper = mapper;
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
        => _set.AddRange(entities);

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
       => _set.UpdateRange(entities);

    public virtual void AttachRange(IEnumerable<TEntity> entities)
       => _set.AttachRange(entities);

    /// <summary>
    /// Получить запись с примененными фильтрами и связанными свойствами.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="readOnly"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> GetFilteredWithIncludes(Expression<Func<TEntity, bool>> filter, bool readOnly = false, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = readOnly ? BaseQuery.AsNoTracking() : BaseQuery;

        if (includes is not null && includes.Any())
        {
            foreach (var include in includes)
                query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(filter);
    }

    /// <summary>
    /// Получить список записей ДТО при помощи проекции сущностей.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<List<TDto>> GetProjectedListAsync<TDto>(Expression<Func<TEntity, bool>> filter = null) where TDto : IMapFrom<TEntity>
    {
        var query = BaseQuery;
        if (filter is not null)
            query = query.Where(filter);

        return await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetFilteredListWithIncludes(Expression<Func<TEntity, bool>> filter, bool readOnly = false, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = readOnly ? BaseQuery.AsNoTracking() : BaseQuery;

        if (includes is not null && includes.Any())
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        if (filter is not null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }
}