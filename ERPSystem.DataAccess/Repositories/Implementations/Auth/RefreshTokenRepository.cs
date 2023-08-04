

using ERPSystem.DataAccess.Entities.Auth;
using ERPSystem.DataAccess.Repositories.Interfaces.Auth;


namespace ERPSystem.DataAccess.Repositories.Implementations.Auth
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
