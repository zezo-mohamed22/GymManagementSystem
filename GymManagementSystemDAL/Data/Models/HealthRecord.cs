using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = default!; 
        public string? Note { get; set; } = default!;
        #region relationship 
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }
        #endregion
    }
}
