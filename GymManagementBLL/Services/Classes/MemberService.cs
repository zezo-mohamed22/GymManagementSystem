using AutoMapper;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService :  IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;
        public MemberService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            //check email 
            //check phone 
            // mapping view model 
            // add to database 
            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone, ct);
            if (emailExists || phoneExists)
            {
                return false; 
            }
            var photo = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(),model.PhotoFile.FileName,"MembersPictures",ct);
            if (string.IsNullOrEmpty(photo))
            {
                return false;
            }
            var member = _mapper.Map<CreateMemberViewModel, Member>(model);
            member.Photo = photo;
            _unitOfWork.GetRepository<Member>().AddAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result == 0)
            {
                 _attachmentService.Delete(member.Phone, "MembersPicutre");
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);
            if (!members.Any())
            {
                return [];
            }
            var membersViewModel = _mapper.Map<IEnumerable< MemberViewModel >>(members);
           
            return membersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int MemberId, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if(member is null)
            {
                return null; 
            }
            var memberViewModel = _mapper.Map<MemberViewModel>(member); 
            var membership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(M=>M.MemberId == MemberId && M.EndDate>DateTime.Now ,ct:ct)   ;
            if(membership is not null)
            {
                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(membership.PlanId, ct);
                memberViewModel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();
                memberViewModel.PlanName = plan?.Name!;
            }
            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(H => H.MemberId == memberId, ct: ct);
            if(healthRecord is null)
            {
                return null;
            }
            return _mapper.Map<HealthRecordViewModel>(healthRecord);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if(member is null)
            {
                return null;
            }
            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false;
            var futureSession = await _unitOfWork.GetRepository<Booking>().AnyAsync(B => B.MemberId == memberId && B.Session.StartDate > DateTime.Now,ct:ct);
            if (futureSession)
            {
                return false;
            }
            _unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result =await _unitOfWork.SaveChangesAsync(ct);
            if (result > 0)
            {
                if (!string.IsNullOrEmpty(member.Photo))
                {
                    _attachmentService.Delete(member.Photo, "MemberPictures");
                }
                return true;
            }
            return false; 
        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false; 
            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Email == model.Email && M.Id!=memberId, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(M => M.Phone == model.Phone&&M.Id != memberId, ct);
            if (emailExists || phoneExists)
            {
                return false;
            }
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdateAt = DateTime.Now;
            _unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0; 
        }
    }
}
