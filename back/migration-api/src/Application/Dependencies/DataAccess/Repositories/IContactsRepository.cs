using Application.Dependencies.DataAccess.Repositories.General;
using Domain.Entities;

namespace Application.Dependencies.DataAccess.Repositories;

public interface IContactsRepository : IGuidRepository<ContactsEntity>
{
}
