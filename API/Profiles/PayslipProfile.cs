using API.DTOs.LogOtDTOs;
using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class PayslipProfile : Profile
    {
        public PayslipProfile()
        {
            CreateMap<Payslip, PayslipDTO>().ReverseMap();

            CreateMap<PersonnelContract, PersonnelContractDTO>().ReverseMap();

            CreateMap<ContractType, ContractTypeDTO>();
            CreateMap<Allowance, AllowancesDTO>();
            CreateMap<AllowanceType, AllowanceTypeDTO>();
            CreateMap<SalaryType, SalaryTypeDTO>();

            CreateMap<LogOt, LogOtDTO>();
            CreateMap<CreateLogOtDTO, LogOt>();
            
        }
    }
}