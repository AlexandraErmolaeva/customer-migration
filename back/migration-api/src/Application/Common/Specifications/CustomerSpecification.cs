using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Common.Specifications;

public class CustomerSpecification : IBaseSpecification<CustomerEntity>
{
    public Func<IQueryable<CustomerEntity>, IOrderedQueryable<CustomerEntity>> Sorter => q => q.OrderByDescending(o => o.CreatedAt);

    public Expression<Func<CustomerEntity, bool>> ToExpressions() => null;
}
