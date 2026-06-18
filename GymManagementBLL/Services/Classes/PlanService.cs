using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var Plans = await _planRepo.GetAllAsync(ct: ct);
            var PlansViewModels = Plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                isActive = p.isActive,
                DurationDays = p.DurationDays
            });
            return PlansViewModels;
        }
        

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();
            var p = await _planRepo.GetByIdAsync(PlanId, ct);
            return p is null? null : new PlanViewModel() {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                isActive = p.isActive,
                DurationDays = p.DurationDays
            };
        }
        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await _planRepo.GetByIdAsync(PlanId, ct);
            if (await HasActiveMembershipsAsync(PlanId, ct)) return null;
            if(plan is null || !plan.isActive)
            {
                return null; 
            }
            return new UpdatePlanViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price 
            };
        }

        public async Task<bool> ToggleActivationAsync(int PlanId, CancellationToken ct = default)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await _planRepo.GetByIdAsync(PlanId, ct);
            if (plan is null) return false;
            if(plan.isActive&& await HasActiveMembershipsAsync(PlanId, ct))
            {
                return false;
            }
            plan.isActive = !plan.isActive;
            plan.UpdateAt = DateTime.Now;
                _planRepo.UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0; 
        }

        public async Task<bool> UpdatePlanAsync(int PlanId, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var _planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await _planRepo.GetByIdAsync(PlanId);
            if (plan is null) return false;
            if (await HasActiveMembershipsAsync(PlanId, ct))
            {
                return false; 
            }
            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdateAt = DateTime.Now;
            _planRepo.UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0; 

        }

        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<Membership>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now,ct);
        }
    }
}
