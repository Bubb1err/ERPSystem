
namespace ERPSystem.DataAccess.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IGenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    }
}
