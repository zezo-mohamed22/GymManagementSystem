using GymManagementSystemDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystemDAL.Data.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar").HasMaxLength(50);
            builder.Property(p => p.Description)
                .HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(p => p.Price).
                HasPrecision(10, 2);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GetDate()");
            builder.ToTable(TB =>
            {
                TB.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");
            });
        }

    }
}
