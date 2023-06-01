using API.DTOs.LogOtDTOs;
using API.DTOs.PayslipDTOs;
<<<<<<< HEAD
using API.DTOs.PersonnelContractDTO;
=======
using API.DTOs.UserInforDTO;
>>>>>>> 4c576048b92eab4967f401dfd08038e435b99cc1
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class PayslipProfile : Profile
    {
        public PayslipProfile()
        {
<<<<<<< HEAD
            CreateMap<Payslip, PayslipDTO>().ReverseMap();

            CreateMap<PersonnelContract, PersonnelContractDTO>().ReverseMap();

            CreateMap<ContractType, ContractTypeDTO>();
            CreateMap<Allowance, AllowancesDTO>();
            CreateMap<AllowanceType, AllowanceTypeDTO>();
            CreateMap<SalaryType, SalaryTypeDTO>();

            CreateMap<LogOt, LogOtDTO>();
            CreateMap<CreateLogOtDTO, LogOt>();
            
=======
            CreateMap<Payslip, PayslipDTO>();
            CreateMap<PayslipDTO, Payslip>();

            CreateMap<UserInfor, UserInforDto>();
            CreateMap<UserInforDto, UserInfor>();

            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
>>>>>>> 4c576048b92eab4967f401dfd08038e435b99cc1
        }
    }
}