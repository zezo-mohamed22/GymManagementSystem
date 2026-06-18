using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public class Membership : BaseEntity
    {

        #region relationship
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; } = default!; 
        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; } = default!;
        #endregion
        public DateTime EndDate { get; set; }
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;
    }
}
