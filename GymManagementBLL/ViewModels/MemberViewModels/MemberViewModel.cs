using GymManagementSystemDAL.Data.Models.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!; 
        public string Email { get; set; } = default!; 
        public string Phone { get; set; } = default!; 
        public string? Photo { get; set; } = default!; 
        public string Gender { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string MembershipStartDate { get; set; } = default!;
        public string MembershipEndDate { get; set; } = default!;
        public string PlanName { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!; 
    }
}
