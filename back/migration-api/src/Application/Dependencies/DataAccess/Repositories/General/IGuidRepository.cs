using Application.Common.Mappings;
using Domain.Entities.General;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Dependencies.DataAccess.Repositories.General;

public interface IGuidRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity> GetFilteredWithIncludes(Expression<Func<TEntity, bool>> filter, bool readOnly = false, params Expression<Func<TEntity, object>>[] includes);
    Task<List<TEntity>> GetFilteredListWithIncludes(Expression<Func<TEntity, bool>> filter, bool readOnly = false, params Expression<Func<TEntity, object>>[] includes);
    Task<List<TDto>> GetProjectedListAsync<TDto>(Expression<Func<TEntity, bool>> filter = null) where TDto : IMapFrom<TEntity>;
    void AddRange(IEnumerable<TEntity> entities);
    void UpdateRange(IEnumerable<TEntity> entities);
    void AttachRange(IEnumerable<TEntity> entities);
}