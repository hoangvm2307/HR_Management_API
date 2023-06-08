using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CandidateDTO;
using API.DTOs.CandidateSkillDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class CandidateSkillProfile : Profile
    {
        public CandidateSkillProfile()
        {
            CreateMap<CandidateSkillDto, CandidateSkill>().ReverseMap();
            CreateMap<CandidateSkillUpdateDto, CandidateSkill>().ReverseMap();
            CreateMap<CandidateSkillCreateDto, CandidateSkill>().ReverseMap();
        }
    }
}