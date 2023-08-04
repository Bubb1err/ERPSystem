using ERPSystem.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.DataAccess.EntityTypeConfigs
{
    internal class UserProfileEntityTypeConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasMany(u => u.Skills)
                .WithMany(s => s.Users);
            builder.HasOne(p => p.User)
                .WithOne(u => u.UserProfile)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
