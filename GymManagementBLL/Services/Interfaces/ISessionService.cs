using GymManagementBLL.Helper;
using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel?>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropdownListAsync(CancellationToken ct);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropdownListAsync(CancellationToken ct);
        Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct);
        Task<UpdateSessionViewModel?> GetSessionToUpdate(int SessionId, CancellationToken ct);
        Task<Result?> UpdateSessionAsync(int SessionId,UpdateSessionViewModel model
            , CancellationToken ct);
        Task<Result> RemoveSessionAsync(int SessionId, CancellationToken ct = default);

    }
}
