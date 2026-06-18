using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementSystemDAL.Data.Models;
using GymManagementSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainerService(IUnitOfWork unitOfWork)
        {
          
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var emailExists = await _trainerRepo.AnyAsync(M => M.Email == model.Email, ct);
            var phoneExists = await _trainerRepo.AnyAsync(M => M.Phone == model.Phone, ct);
            if (emailExists || phoneExists)
            {
                return false;
            }
            var trainer = new Trainer()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Specialties = model.Specialties
            };
            _trainerRepo.AddAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainers = await _trainerRepo.GetAllAsync(ct: ct);
            if (!trainers.Any())
            {
                return [];
            }
            var trainersViewModel = trainers.Select(M => new TrainerViewModel
            {
                Id = M.Id,
                Name = M.Name,
                Email = M.Email,
                Phone = M.Phone,
                Gender = M.Gender,
                specialties = M.Specialties
            });
            return trainersViewModel;
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int TrainerId, CancellationToken ct)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await _trainerRepo.GetByIdAsync(TrainerId, ct);
            if (trainer is null)
            {
                return null;
            }
            var trainerViewModel = new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Gender = trainer.Gender,
                Phone = trainer.Phone,
                Email = trainer.Email,
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                specialties = trainer.Specialties

            };
            return trainerViewModel;
        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;
            var futureSession = await _unitOfWork.GetRepository<Session>().AnyAsync(B => B.TrainerId == trainerId && B.StartDate > DateTime.Now, ct: ct);
            if (futureSession)
            {
                return false;
            }
            _trainerRepo.DeleteAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

     

        public async Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;
            var emailExists = await _trainerRepo.AnyAsync(M => M.Email == model.Email && M.Id != trainerId, ct);
            var phoneExists = await _trainerRepo.AnyAsync(M => M.Phone == model.Phone && M.Id != trainerId, ct);
            if (emailExists || phoneExists)
            {
                return false;
            }
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.UpdateAt = DateTime.Now;
            trainer.Specialties = trainer.Specialties;
            _trainerRepo.UpdateAsync(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct) ;
            return result > 0;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct)
        {
            var _trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await _trainerRepo.GetByIdAsync(trainerId, ct);
            if (trainer is null)
            {
                return null;
            }
            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
            };
        }
    }
}
