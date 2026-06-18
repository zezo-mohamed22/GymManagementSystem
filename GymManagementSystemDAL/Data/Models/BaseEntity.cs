using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public abstract class BaseEntity
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public DateTime? UpdateAt { set; get; }
    }
}
