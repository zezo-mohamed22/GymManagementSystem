using GymManagementSystemDAL.Data.DbContexts;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemDAL.Repositories.Classes
{
    public class SessionRepoistory : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepoistory(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var sessions = _dbContext.Sessions.Include(s => s.Category).Include(s => s.Trainer);
            return await sessions.ToListAsync(ct);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct)
        {
            return await _dbContext.Bookings.CountAsync(B => B.SessionId == sessionId, ct);
        }

        public async Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct)
        {
            return await _dbContext.Sessions.AsNoTracking().Include(s => s.Category).Include(s => s.Trainer).FirstOrDefaultAsync(s=>s.Id == SessionId);
        }
    }
}
