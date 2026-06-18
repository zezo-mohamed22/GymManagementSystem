using GymManagementSystemDAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public Gender Gender { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!;
        public Specialties specialties { get; set; }
    }
}
