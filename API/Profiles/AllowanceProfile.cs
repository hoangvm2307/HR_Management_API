using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.AllowanceDTO;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class AllowanceProfile : Profile
    {
        public AllowanceProfile()
        {
            CreateMap<Allowance, AllowanceDTO>();
            CreateMap<AllowanceCreationDTO, Allowance>();
            CreateMap<AllowanceUpdateDTO, Allowance>();
            CreateMap<Allowance, AllowanceUpdateDTO>();
            CreateMap<AllowanceType, AllowanceTypeDTO>();

        }
    }
}