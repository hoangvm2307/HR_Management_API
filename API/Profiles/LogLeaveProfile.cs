using API.DTOs.LeaveDayDetailDTO;
using API.DTOs.LeaveTypeDTO;
using API.DTOs.LogLeaveDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class LogLeaveProfile : Profile
    {
        public LogLeaveProfile()
        {
            CreateMap<LogLeave, LogLeaveDTO>();
            CreateMap<LeaveType, LeaveTypeDTO>();

            CreateMap<LeaveType, LeaveTypeWithLogLeavesDTO>();
            CreateMap<LeaveTypeCreationDTO, LeaveType>();
            CreateMap<LeaveTypeUpdateDTO, LeaveType>();

            CreateMap<LogLeave, LeaveTypeLogDTO>();
            

            CreateMap<LogLeaveCreationDTO, LogLeave>();
            CreateMap<LogLeaveUpdateDTO, LogLeave>();
            CreateMap<LogLeave, LogLeaveUpdateDTO>();

            CreateMap<LeaveDayDetail, LeaveDayDetailDTO>();
            CreateMap<LeaveType, LeaveDayDetailTypeDTO>();
            CreateMap<LeaveDayDetailCreationDTO, LeaveDayDetail>();
        }
    }
}