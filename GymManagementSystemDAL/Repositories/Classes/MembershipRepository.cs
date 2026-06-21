using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Classes
{
    public class MembershipRepository : IMembershipRepository
    {
        public Task<List<Membership>> GetMembershipsWithMembersAndPlansAsync(Expression<Func<Membership, bool>>? predicate, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
