using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace GymManagementSystemPL.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]

    public class MembersController : Controller
    {
        private readonly IMemberService _memberServ;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberServ,IAttachmentService attachmentService)
        {
            _memberServ = memberServ;
            _attachmentService = attachmentService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        
       {
            var members = await _memberServ.GetAllMembersAsync(ct);
            return View(members);
        }
        // memberDetails - display member profile 
        // Url/member/details/{id}

        #region create member 
        // get 
        // create - post 
        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model , CancellationToken ct)
        {
            bool value = !ModelState.IsValid;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _memberServ.CreateMemberAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Member"; 
            }
                return RedirectToAction(nameof(Index)); 
        }
        #endregion
        #region GetMember
        // Get baseUrl/Members/MemberDetails/{Id}
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberServ.GetMemberDetailsAsync(id, ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not found";
                return RedirectToAction(nameof(Index));
            } 
            return View(member);
        }
        // MemberDetails - Displays one member 
        // GetBaseURL / members /HealthRecordDetails/[id]
        public async Task<IActionResult> HealthRecordDetails(int id,CancellationToken ct)
        {
            var healthRecord = await _memberServ.GetMemberHealthRecordAsync(id, ct);
            if(healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record not found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord); 
        }
        #endregion
        #region edit member 
         public async Task<IActionResult> EditMember(int id , CancellationToken ct)
        {
            var member = await _memberServ.GetMemberToUpdateAsync(id,ct);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> EditMemberAsync([FromRoute]int id , MemberToUpdateViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _memberServ.UpdateMemberAsync(id, model, ct);
            if (result == false)
            {
                TempData["ErrorMessage"] = "Failed to Update Member";
                return View(model);
            }
            TempData["SuccessMessage"] = "Member updated successfully";

            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region delete
        public async Task<IActionResult> DeleteAsync(int id , CancellationToken ct)
        {
            var member = await _memberServ.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedAsync(int id, CancellationToken ct)
        {
            var result = await _memberServ.RemoveMemberAsync(id, ct);
            if (result) {
                TempData["SuccessMessage"] = "Removed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member cannot be removed";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        public async Task<IActionResult> PictureAsync(int id ,CancellationToken ct)
        {
            var member = await _memberServ.GetMemberDetailsAsync(id, ct);
            if(member is null || string.IsNullOrEmpty(member.Photo))
            {
                return NotFound();
            }
            var result = _attachmentService.GetFile(member.Photo, "MembersPictures");
            if(result is null)
            {
                return NotFound();
            }
            return File(result.Value.stream, result.Value.ContentType);
        }
    }
}
