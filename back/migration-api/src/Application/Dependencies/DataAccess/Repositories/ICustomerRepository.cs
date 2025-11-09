using Application.Dependencies.DataAccess.Repositories.General;
using Domain.Entities;

namespace Application.Dependencies.DataAccess.Repositories;

public interface ICustomerRepository : IGuidRepository<CustomerEntity>
{
}
