using API.DTOs.StaffDtos;
using API.DTOs.UserInforDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class StaffInfoProfile : Profile
    {
        public StaffInfoProfile()
        {
            CreateMap<UserInfor, StaffInfoDto>().ReverseMap();
        }
    }
}
