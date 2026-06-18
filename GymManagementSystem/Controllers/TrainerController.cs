
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    [Authorize]
    [Authorize(Roles = "SuperAdmin")]

    [Route("Trainers")]
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerServ;

        public TrainersController(ITrainerService trainerServ)
        {
            _trainerServ = trainerServ;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerServ.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        #region Create

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _trainerServ.CreateTrainerAsync(model, ct);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result ? "Trainer Created Successfully" : "Failed To Create Trainer";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Details

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerServ.GetTrainerDetailsAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        #endregion

        #region Edit

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await _trainerServ.GetTrainerToUpdateAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            TrainerToUpdateViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _trainerServ.UpdateTrainerAsync(id, model, ct);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer";
                return View(model);
            }

            TempData["SuccessMessage"] = "Trainer Updated Successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerServ.GetTrainerDetailsAsync(id, ct);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerServ.RemoveTrainerAsync(id, ct);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result ? "Trainer Removed Successfully" : "Trainer Cannot Be Removed";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
