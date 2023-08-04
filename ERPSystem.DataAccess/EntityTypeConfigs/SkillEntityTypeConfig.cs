using ERPSystem.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPSystem.DataAccess.EntityTypeConfigs
{
    internal class SkillEntityTypeConfig : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasMany(s => s.Users)
                .WithMany(u => u.Skills);
        }
    }
}
