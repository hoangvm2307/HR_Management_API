using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.DepartmentDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentCreateDto, Department>().ReverseMap();
            CreateMap<Department, DepartmentUserDto>().ReverseMap();
        }
    }
}