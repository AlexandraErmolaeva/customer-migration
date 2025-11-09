using Application.Dependencies.DataAccess.Repositories;

namespace Application.Dependencies.DataAccess;

public interface IUnitOfWork
{
    public IContactsRepository Contacts { get; }
    public ICustomerRepository Customers { get; }
    public IFinancialProfileRepository FinancialProfiles { get; }

    bool HasActiveTransaction { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    void Dispose();
    Task RollbackTransactionAsync();
    Task SaveChangesAsync();
}
