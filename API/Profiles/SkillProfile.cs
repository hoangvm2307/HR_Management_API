using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.SkillDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill,SkillDto>().ReverseMap();
            CreateMap<Skill,SkillCreateDto>().ReverseMap();
            CreateMap<Skill,SkillUpdateDto>().ReverseMap();

            CreateMap<StaffSkill, StaffSkillDto>().ReverseMap();
            CreateMap<StaffSkill,StaffSkillCreateDto>().ReverseMap();
            CreateMap<StaffSkill,StaffSkillUpdateDto>().ReverseMap();
        }
    }
}