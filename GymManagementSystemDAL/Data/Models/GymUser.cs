using GymManagementSystemDAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Data.Models
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email  { get; set; } = default!;
        public string Phone  { get; set; } = default!;
        public DateOnly DateOfBirth  { get; set; } = default!;
        public Gender Gender { get;set;}
        public Address Address { get; set; } = default!; 
    }
}
