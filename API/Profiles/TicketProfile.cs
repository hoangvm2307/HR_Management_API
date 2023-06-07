using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.TicketDTO;
using API.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<TicketType, TicketTypeCreateDto>().ReverseMap();
            CreateMap<Ticket,TicketCreateDto>().ReverseMap();
            CreateMap<Ticket,TicketUpdateDto>().ReverseMap();
        }
    }
}