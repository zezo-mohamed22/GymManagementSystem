using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
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
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnalyticsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var upcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > now);
            var ongoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate <= now && s.EndDate>=now);
            var completedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate < now);
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync();
            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync();
            var activeMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(m=>m.EndDate>now);
            return new AnalyticsViewModel()
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };
        }
    }
}
