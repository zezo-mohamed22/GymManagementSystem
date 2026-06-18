using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Classes;
using GymManagementSystemDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
    [Authorize]

    public class PlansController : Controller
    {
        private readonly IPlanService _planService;
        public PlansController(IPlanService plan)
        {
            _planService = plan;
        }

        public async Task<IActionResult> Index(CancellationToken ct) => View(await _planService.GetAllPlansAsync(ct: ct));
        [HttpGet]
        public async Task<IActionResult> Details(int id , CancellationToken ct)
        {
            var plan = await _planService.GetPlanByIdAsync(id,ct);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (not found , inactive , or has active membership)";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id , UpdatePlanViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _planService.UpdatePlanAsync(id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Plan failed to update";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Activate (int id , CancellationToken ct)
        {
            var result = await _planService.ToggleActivationAsync(id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan status changed"; 
            }
            TempData["ErrorMessage"] = "Failed to toggle Plan status";
            return RedirectToAction(nameof(Index));
        }
    }
}
