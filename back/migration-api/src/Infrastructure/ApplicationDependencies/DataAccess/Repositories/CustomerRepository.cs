using Application.Common.Mappings;
using Application.Common.Specifications;
using Application.Dependencies.DataAccess.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories.General;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal sealed class CustomerRepository : GuidRepositoryBase<CustomerEntity>, ICustomerRepository
{
    protected override IQueryable<CustomerEntity> BaseQuery => _context.Customers;

    public CustomerRepository(CustomersDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<(IEnumerable<TDto> Customers, int Count)> GetPaginatedProjectionAsync<TDto>(
        IBaseSpecification<CustomerEntity> spec,
        int take, int page) where TDto : IMapFrom<CustomerEntity>
    {
        var query = BaseQuery;
        var filter = spec.ToExpressions();
        if (filter is not null)
            query = query.Where(filter);

        query = spec.Sorter is not null ? spec.Sorter(query) : query;

        var count = await query.CountAsync();
        query = query
            .Skip((page - 1) * take)
            .Take(take);
        var customers =  await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
        return (customers, count);
    }
}
