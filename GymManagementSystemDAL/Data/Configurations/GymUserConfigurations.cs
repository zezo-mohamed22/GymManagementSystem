using GymManagementSystemDAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Configurations
{
    public class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name).
                HasColumnType("varchar").
                HasMaxLength(50);
            builder.Property(u => u.Email).
                HasColumnType("varchar").
                HasMaxLength(100);
            builder.Property(u => u.Phone).
                HasColumnType("varchar").
                HasMaxLength(11);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Phone).IsUnique();
            builder.ToTable(TB =>
            {
                TB.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                TB.HasCheckConstraint(
                    "PhoneCheck",
                    "Phone LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'"
                );
            });
            builder.OwnsOne(u => u.Address, address => 
            {
                address.Property(A => A.Street).HasColumnType("varchar").HasMaxLength(30);
                address.Property(A => A.City).HasColumnType("varchar").HasMaxLength(30);
            });
        }
    }
}
