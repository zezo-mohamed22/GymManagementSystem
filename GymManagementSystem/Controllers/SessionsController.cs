using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace GymManagementSystemPL.Controllers
{
    [Authorize]

    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);

            return View();
        }

        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList( await _sessionService.GetTrainerForDropdownListAsync(ct),"Id","Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropdownListAsync(ct) ,"Id","CategoryName") ;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }
            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.Suceess)
            {

                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropDownAsync(ct);
            return View(model);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        public async Task<IActionResult> Edit( int id , CancellationToken ct) 
        {
            var session = await _sessionService.GetSessionToUpdate(id,ct);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session not found Or Cannot be updated";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownAsync(ct);
            return View(session);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }
            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.Suceess)
            {

                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropDownAsync(ct);
            return View(model);

        }
        public async Task<IActionResult> DeleteAsync(int id ,CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session not found Or Cannot be deleted";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id,CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id);
            if (result.Suceess)
            {
                TempData["SuccessMessage"] = "Session deleted Successfully";
            }
            else TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));

        }
    }
}
