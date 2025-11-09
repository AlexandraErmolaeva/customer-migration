using Application.Common.Mappings;
using Application.Common.Specifications;
using Application.Dependencies.DataAccess.Repositories.General;
using Domain.Entities;

namespace Application.Dependencies.DataAccess.Repositories;

public interface ICustomerRepository : IGuidRepository<CustomerEntity>
{
    Task<(IEnumerable<TDto> Customers, int Count)> GetPaginatedProjectionAsync<TDto>(
        IBaseSpecification<CustomerEntity> spec,
        int take, int page) where TDto : IMapFrom<CustomerEntity>;
}
