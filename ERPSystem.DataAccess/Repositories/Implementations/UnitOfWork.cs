using ERPSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Transactions;

namespace ERPSystem.DataAccess.Repositories.Implementations
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext
    {
        private bool _disposed;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
        }
        public TContext DbContext { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            if (hasCustomRepository)
            {
                var customRepo = DbContext.GetService<IGenericRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(DbContext);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public IRepository GetRepository<TEntity, IRepository>(bool hasCustomRepository = false) 
            where TEntity : class
            where IRepository : class, IGenericRepository<TEntity>
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            if (hasCustomRepository)
            {
                var customRepo = DbContext.GetService<IRepository>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(DbContext);
            }

            return (IRepository)_repositories[type];
        }

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();

                    DbContext.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
