using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public class Booking : BaseEntity   
    {
        #region relationship 
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; } = default!;
        public Session Session { get; set; } = default!;
        public int SessionId { get; set; }
        #endregion
        public bool IsAttended { get; set; }

    }
}
