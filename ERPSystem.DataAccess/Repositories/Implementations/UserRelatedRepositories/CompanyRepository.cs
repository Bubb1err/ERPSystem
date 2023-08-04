using ERPSystem.DataAccess.Entities.UserEntities;
using ERPSystem.DataAccess.Repositories.Interfaces.UserRelatedRepositories;

namespace ERPSystem.DataAccess.Repositories.Implementations.UserRelatedRepositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
