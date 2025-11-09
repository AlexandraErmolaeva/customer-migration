using Application.Dependencies.DataAccess.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories.General;
using Infrastructure.Persistence.Context;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal sealed class CustomerRepository : GuidRepositoryBase<CustomerEntity>, ICustomerRepository
{
    protected override IQueryable<CustomerEntity> BaseQuery => _context.Customers;

    public CustomerRepository(CustomersDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
