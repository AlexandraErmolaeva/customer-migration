using Domain.Entities.General;
using System.Linq.Expressions;

namespace Application.Common.Specifications;

public interface IBaseSpecification<T> where T : EntityBase
{
    Expression<Func<T, bool>> ToExpressions();
    Func<IQueryable<T>, IOrderedQueryable<T>> Sorter { get; }
}
