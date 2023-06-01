using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.PayslipDTOs;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class PayslipProfile : Profile
    {
        public PayslipProfile()
        {
            // CreateMap<Entities.PointOfInterest, Models.PointOfInterestDTO>();
            // CreateMap<Models.PointOfInterestCreationDTO, Entities.PointOfInterest>();
            // CreateMap<Models.PointOfInterestUpdateDTO, Entities.PointOfInterest>();
            // CreateMap<Entities.PointOfInterest, Models.PointOfInterestUpdateDTO>();

            CreateMap<Payslip, PayslipDTO>();
            CreateMap<PayslipDTO, Payslip>();

        }
    }
}