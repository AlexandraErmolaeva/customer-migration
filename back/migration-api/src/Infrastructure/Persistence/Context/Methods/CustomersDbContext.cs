using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.Persistence.Context;

public partial class CustomersDbContext
{
    public bool HasActiveTransaction => _currentTransaction is not null;
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
            throw new InvalidOperationException("Невозможно открыть транзакцию повторно.");

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("Нет открытой транзакции для коммита.");

        try
        {
            await SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("Нет открытой транзакции для отката.");

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}