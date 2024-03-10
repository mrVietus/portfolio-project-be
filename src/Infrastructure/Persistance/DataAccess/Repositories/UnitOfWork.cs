using Crawler.Application.Common.Interfaces;
using Crawler.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Crawler.Infrastructure.Persistance.DataAccess.Repositories;

public sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork, IDisposable
{
    private bool _disposed;
    private IDbContextTransaction? _transaction;

    public async Task CreateTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) 
        => await dbContext.SaveChangesAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken = default) 
        => await _transaction!.CommitAsync(cancellationToken);

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction!.RollbackAsync(cancellationToken);
        _transaction.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                dbContext.Dispose();
        _disposed = true;
    }
}
