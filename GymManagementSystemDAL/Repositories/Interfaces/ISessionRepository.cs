using GymManagementSystemDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session?>> GetAllSessionWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct);

        Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId , CancellationToken ct);
    }
}
