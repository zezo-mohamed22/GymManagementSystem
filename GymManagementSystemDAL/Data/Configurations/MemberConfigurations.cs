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
    public class MemberConfigurations : GymUserConfigurations<Member>,IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(M => M.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GetDate()");
            base.Configure(builder);
        }
    }
}
