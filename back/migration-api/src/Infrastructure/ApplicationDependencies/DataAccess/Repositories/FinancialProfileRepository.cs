using Application.Dependencies.DataAccess.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.ApplicationDependencies.DataAccess.Repositories.General;
using Infrastructure.Persistence.Context;

namespace Infrastructure.ApplicationDependencies.DataAccess.Repositories;

internal sealed class FinancialProfileRepository : GuidRepositoryBase<FinancialProfileEntity>, IFinancialProfileRepository
{
    protected override IQueryable<FinancialProfileEntity> BaseQuery => _context.FinancialProfiles;

    public FinancialProfileRepository(CustomersDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
