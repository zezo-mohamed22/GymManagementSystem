using GymManagementSystemDAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }
        #region 
        public ICollection<Session> Sessions { get; set; } = default!;
        #endregion
    }
}
