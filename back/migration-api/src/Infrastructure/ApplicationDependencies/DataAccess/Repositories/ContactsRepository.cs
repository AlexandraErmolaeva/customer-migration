using Application.Dependencies.DataAccess.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories.General;
using Infrastructure.Persistence.Context;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal sealed class ContactsRepository : GuidRepositoryBase<ContactsEntity>, IContactsRepository
{
    protected override IQueryable<ContactsEntity> BaseQuery => _context.Contacts;

    public ContactsRepository(CustomersDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
