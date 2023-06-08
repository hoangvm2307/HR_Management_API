using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CandidateDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<CandidateDto,Candidate>().ReverseMap();
            CreateMap<CandidateCreateDto,Candidate>().ReverseMap();
            CreateMap<CandidateUpdateDto,Candidate>().ReverseMap();
        }
    }
}