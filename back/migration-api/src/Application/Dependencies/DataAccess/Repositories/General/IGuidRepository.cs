using Application.Common.Mappings;
using Domain.Entities.General;
using System.Linq.Expressions;

namespace Application.Dependencies.DataAccess.Repositories.General;

public interface IGuidRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity> GetById(Guid id, bool readOnly = false);
    Task<TEntity> GetFilteredWithIncludes(Expression<Func<TEntity, bool>> filter, bool readOnly = false, params Expression<Func<TEntity, object>>[] includes);
    Task<IEnumerable<TEntity>> GetFilteredList(Expression<Func<TEntity, bool>> filter, bool readOnly = false);
    Task<List<TDto>> GetProjectedListAsync<TDto>(Expression<Func<TEntity, bool>> filter = null) where TDto : IMapFrom<TEntity>;
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
}