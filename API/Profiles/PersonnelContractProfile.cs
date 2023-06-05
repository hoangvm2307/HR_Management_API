using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class PersonnelContractProfile : Profile
    {
        public PersonnelContractProfile()
        {
            
            CreateMap<PersonnelContract, PersonnelContractDTO>().ReverseMap();
            CreateMap<PersonnelContractCreationDTO, PersonnelContract>();
            CreateMap<ContractTypeCreationDTO, ContractType>();
            CreateMap<PersonnelContractUpdateDTO, PersonnelContract>();
            CreateMap<PersonnelContract, PersonnelContractUpdateDTO>();

            CreateMap<ContractType, ContractTypeDTO>();
            CreateMap<Allowance, AllowancesDTO>();
            CreateMap<AllowanceType, AllowanceTypeDTO>();
            
        }
    }
}