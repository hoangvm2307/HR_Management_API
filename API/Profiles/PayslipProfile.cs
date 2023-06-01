using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.PayslipDTOs;
using API.DTOs.UserInforDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class PayslipProfile : Profile
    {
        public PayslipProfile()
        {
            CreateMap<Payslip, PayslipDTO>();
            CreateMap<PayslipDTO, Payslip>();

            CreateMap<UserInfor, UserInforDto>();
            CreateMap<UserInforDto, UserInfor>();

            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}