using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; } = default!;
        #region relationships 
        public HealthRecord HealthRecord { get; set; } = default!;
        public ICollection<Membership> Memberships { get; set; } = default!; 
        public ICollection<Booking> Bookings { get; set; } = default!; 
      
        #endregion
    }
}
