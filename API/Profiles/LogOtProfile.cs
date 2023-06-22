using API.DTOs.LogOtDTOs;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class LogOtProfile: Profile
    {
        public LogOtProfile()
        {
            CreateMap<UserInfor, UserInfoLogOt>();

            CreateMap<LogOt, LogOtDTO>();
            CreateMap<LogOtCreationDTO, LogOt>();
            CreateMap<LogOtUpdateDTO, LogOt>();
            CreateMap<LogOt, LogOtUpdateDTO>();


            CreateMap<OtType, OtTypeDTO>();
            CreateMap<OtDetail, OtDetailDTO>();
        }
    }
}
