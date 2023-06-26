using API.DTOs.DepartmentDTO;
using API.DTOs.LeaveTypeDTO;
using API.DTOs.LogOtDTOs;
using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.DTOs.TicketDTO;

using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class PayslipProfile : Profile
    {
        public PayslipProfile()
        {
            CreateMap<Payslip, PayslipDTO>().ReverseMap();
            CreateMap<Ticket, TicketDto>().ReverseMap();
            
            CreateMap<PersonnelContract, PersonnelContractDTO>().ReverseMap();
            CreateMap<ContractType, ContractTypeDTO>();
            CreateMap<Allowance, AllowanceDTO>();
            CreateMap<AllowanceType, AllowanceTypeDTO>().ReverseMap();

            CreateMap<SalaryType, SalaryTypeDTO>();

            CreateMap<LogOt, LogOtDTO>();
            CreateMap<LogOtCreationDTO, LogOt>();
            CreateMap<LogOtUpdateDTO, LogOt>();
            CreateMap<LogOt, LogOtUpdateDTO>();
            
            CreateMap<Payslip, PayslipDTO>();
            CreateMap<PayslipCreationDTO, Payslip>();
            CreateMap<PayslipDTO, Payslip>();

            CreateMap<TaxDetail, TaxDetailDTO>();
            CreateMap<TaxDetailCreationDTO, TaxDetail>();
            CreateMap<TaxList, TaxListDTO>();

            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();

            // CreateMap<Department, DepartmentDto>();
            // CreateMap<DepartmentDto, Department>();

            //CreateMap<UserInfor, DepartmentUserInforDto>();
            CreateMap<UserInfor, DepartmentUserInforDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>();
            
        }
    }
}