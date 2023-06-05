using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.AllowanceDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class AllowanceProfile : Profile
    {
        public AllowanceProfile()
        {
            CreateMap<Allowance, AllowanceDTO>();
        }
    }
}