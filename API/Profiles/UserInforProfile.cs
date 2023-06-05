using API.DTOs.UserInforDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class UserInforProfile : Profile
    {
        public UserInforProfile()
        {
            CreateMap<UserInforDto, UserInfor>().ReverseMap();
        }
    }
}