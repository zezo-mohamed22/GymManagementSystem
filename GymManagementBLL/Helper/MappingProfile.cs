using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemDAL.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapMember();
            MapSession();
        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City
            })).ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
            CreateMap<Member, MemberViewModel>().
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString())).
                ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString())).
                ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));
            CreateMap<Member, MemberToUpdateViewModel>().ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber)).
                ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City)).
                ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));
            CreateMap<MemberToUpdateViewModel, Member>().ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore()).AfterMap((src, dest) =>
                {
                    dest.Address = new Address()
                    {
                        BuildingNumber = src.BuildingNumber,
                        Street = src.Street,
                        City = src.City
                    };
                    dest.UpdateAt = DateTime.Now;
                });
        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>().
                ForMember(dest=>dest.CategoryName, opt=>opt.MapFrom(src =>src.Category.CategoryName)).
                ForMember(dest=>dest.TrainerName,opt=>opt.MapFrom(src=>src.Trainer.Name)).ForMember(
                dest=>dest.AvailableSlots,opt=>opt.Ignore());
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
    }
}
