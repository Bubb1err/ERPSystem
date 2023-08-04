using ERPSystem.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.DataAccess.EntityTypeConfigs
{
    internal class UserEntityTypeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.UserProfile)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);
            builder.OwnsOne(u => u.Company)
                .HasKey(x => x.OwnerId);
        }
    }
}
