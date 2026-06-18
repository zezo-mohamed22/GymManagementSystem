using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public class Session : BaseEntity
    {
        public string Descripiton { get; set; } = default!;
        public int Capacity { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #region relationships
        public Trainer Trainer { get; set; } = default!;
        public int TrainerId { get; set; }
        public Category Category { get; set; } = default!; 
        public int CategoryId { get; set; }
        public ICollection<Booking> Bookings { get; set; } = default!; 
        #endregion
    }
}
