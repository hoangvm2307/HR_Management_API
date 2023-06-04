using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.TicketDTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class TicketEextensions
    {
        public static IQueryable<TicketDto> ProjectTicketToTicketDto(this IQueryable<Ticket> query)
        {
            return query
                    .Select(ticket => new TicketDto
                    {
                        TicketId = ticket.TicketId,
                        StaffId = ticket.StaffId,
                        TicketTypeId = ticket.TicketTypeId,
                        TicketTitle = ticket.TicketFile,
                        TicketFile = ticket.TicketFile,
                        TicketStatus = ticket.TicketStatus,
                        ProcessNote = ticket.ProcessNote,
                        RespondencesId = ticket.RespondencesId,
                        CreateDate = ticket.CreateAt,
                        ChangeStatusTime = ticket.ChangeStatusTime
                    }).AsNoTracking();
        }
    }
}