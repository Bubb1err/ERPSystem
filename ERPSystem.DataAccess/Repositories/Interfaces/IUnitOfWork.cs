
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.DataAccess.Repositories.Interfaces
{
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext DbContext { get; }
       
    }
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
        IRepository GetRepository<TEntity, IRepository>(bool hasCustomRepository = false)
            where TEntity : class
            where IRepository : class, IGenericRepository<TEntity>;
        int SaveChanges(bool ensureAutoHistory = false);
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);
    }
}
