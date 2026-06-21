using GymManagementSystemDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Interfaces
{
    public interface IMembershipRepository 
    {
        Task<List<Membership>> GetMembershipsWithMembersAndPlansAsync(Expression<Func<Membership,bool>>? predicate,CancellationToken ct );
    }
}
