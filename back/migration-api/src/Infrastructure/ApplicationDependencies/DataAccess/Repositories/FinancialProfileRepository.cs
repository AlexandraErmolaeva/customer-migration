using Application.Dependencies.DataAccess.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories.General;
using Infrastructure.Persistence.Context;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal sealed class FinancialProfileRepository : GuidRepositoryBase<FinancialProfileEntity>, IFinancialProfileRepository
{
    public FinancialProfileRepository(CustomersDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    protected override IQueryable<FinancialProfileEntity> BaseQuery => _context.FinancialProfiles;
}
