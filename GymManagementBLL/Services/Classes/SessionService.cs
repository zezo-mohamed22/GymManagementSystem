using AutoMapper;
using GymManagementBLL.Helper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Data.Models.Enums;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("End date must be in the future"); ;
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await trainerRepo.GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("trainer Not found"); ;
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            if (await _unitOfWork.SessionRepository.AnyAsync(s => s.TrainerId == model.TrainerId &&
            ((s.StartDate <= model.StartDate && s.EndDate >= model.StartDate) || (model.StartDate <= s.StartDate && model.EndDate >= s.StartDate)), ct))
            {
                return Result.Fail("Trainer has another session in same time");
            }
            var category = await categoryRepo.GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category not found"); ;

            var isValid =  Enum.TryParse<Specialties>(category.CategoryName, true, out var categorySpecialites);
            if(!isValid || trainer.Specialties!=categorySpecialites){
                return Result.Validation("Cannot create this session for this trainer"); ;
            }

            var session = _mapper.Map<Session>(model);
            _unitOfWork.SessionRepository.AddAsync(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result>0?Result.OK():Result.Fail("Failed To create session"); 
        } 

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategoryAsync(ct);
            if(!sessions.Any()) return [];
           
            var sessionsViewModel = _mapper.Map < IEnumerable < SessionViewModel >>(sessions.OrderByDescending(s => s.StartDate));;
                
            foreach(var session in sessionsViewModel)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }
            return sessionsViewModel;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropdownListAsync(CancellationToken ct)
        {
            var categoryRepo =  _unitOfWork.GetRepository<Category>();
            var categories = await categoryRepo.GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryAsync(SessionId, ct);
            if(session is null)
            {
                return null; 
            }
            var sessionViewModel = _mapper.Map<SessionViewModel>(session);
            sessionViewModel.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            return sessionViewModel;
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdate(int SessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(SessionId, ct);
            if(session is null)
            {
                return null; 
            }
            if (!await IsValidSessionForUpdateAsync(session, ct)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session);
        }
       
        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropdownListAsync(CancellationToken ct)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainers = await trainerRepo.GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<Result> RemoveSessionAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(SessionId);
            if(session is null)
            {
                return Result.NotFound("Session not found");
            }
            if (session.StartDate <= DateTime.Now)
            {
                return Result.Fail("Cannot Delete Ongoing session");
            }
            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            if (bookingCount > 0)
            {
                return Result.Fail("Cannot delete a session  that has bookings");
            }
            _unitOfWork.SessionRepository.DeleteAsync(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to delete session");
        }

        public async Task<Result?> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct)
        {
            var session =await _unitOfWork.SessionRepository.GetByIdAsync(SessionId);
            if(session is null)
            {
                return Result.NotFound("Session is not found");
            }
            if (session.StartDate <= DateTime.Now)
            {
                return Result.Fail("Cannot edit a Session that has already start");
            }
            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            if (bookingCount > 0)
            {
                return Result.Fail("Cannot edit a Session that has bookings");
            }
            if (model.EndDate <= model.StartDate)
            {
                return Result.Validation("End Date must be after start date");
            }
            if(model.StartDate<= DateTime.Now)
            {
                return Result.Validation("Start Date must be in the fature");
            }
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await trainerRepo.GetByIdAsync(model.TrainerId, ct);
            if(trainer is null)
            {
                return Result.NotFound("Trainer Not found");
            }
            var isUnAvaiable = await _unitOfWork.SessionRepository.AnyAsync(S => S.TrainerId == model.TrainerId && S.Id != session.Id &&
            (S.StartDate <= model.StartDate && S.EndDate >= model.StartDate) || (model.StartDate <= S.StartDate && model.EndDate >= S.StartDate), ct);
            if (isUnAvaiable)
            {
                return Result.Fail("Trainer has another session in same time");
            }

            var CategoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await CategoryRepo.GetByIdAsync(session.CategoryId, ct);
            var isValid = Enum.TryParse<Specialties>(category.CategoryName, true, out var categorySpecialites);
            if(isValid || categorySpecialites != trainer.Specialties)
            {
                return Result.Validation("Cannot update this session for this trainer"); ;
            }
            _mapper.Map(model, session);
            session.UpdateAt = DateTime.Now;
            _unitOfWork.SessionRepository.UpdateAsync(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Cannot update session");
        }
        private async Task<bool> IsValidSessionForUpdateAsync(Session session, CancellationToken ct)
        {
            if (session.StartDate <= DateTime.Now)
            {
                return false;
            }
            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return bookingCount == 0;
        }

    }
}
