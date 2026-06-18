using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementSystemDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int MemberId, CancellationToken ct);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct);
        Task<bool> UpdateMemberAsync(int memberId , MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int memberId , CancellationToken ct = default);

    }
}
