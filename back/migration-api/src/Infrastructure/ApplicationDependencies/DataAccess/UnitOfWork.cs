using Application.Dependencies.DataAccess;
using Application.Dependencies.DataAccess.Repositories;
using Infrastructure.Persistence.Context;

namespace Infrastructure.ApplicationDependencies.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly CustomersDbContext _context;

    public IContactsRepository Contacts { get; }
    public ICustomerRepository Customers { get; }
    public IFinancialProfileRepository FinancialProfiles { get; }

    public UnitOfWork(CustomersDbContext dbContext,
        IContactsRepository contacts,
        ICustomerRepository customers,
        IFinancialProfileRepository financialProfiles)
    {
        _context = dbContext;
        Contacts = contacts;
        Customers = customers;
        FinancialProfiles = financialProfiles;
    }

    public bool HasActiveTransaction => _context.HasActiveTransaction;

    public async Task BeginTransactionAsync() => await _context.BeginTransactionAsync();
    public async Task CommitTransactionAsync() => await _context.CommitTransactionAsync();
    public async Task RollbackTransactionAsync() => await _context.RollbackTransactionAsync();
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
