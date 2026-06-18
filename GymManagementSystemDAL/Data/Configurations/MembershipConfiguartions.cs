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
    public class MembershipConfiguartions : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(M => M.Id);
            builder.Property(M => M.CreatedAt).
                HasColumnName("StartDate").
                HasDefaultValueSql("GetDate()");

        }
    }
}
