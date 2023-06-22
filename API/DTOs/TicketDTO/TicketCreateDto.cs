using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.TicketDTO
{
    public class TicketCreateDto
    {
        public int? TicketTypeId { get; set; }

        public string TicketTitle { get; set; } = null!;

        public string TicketReason { get; set; }

        public string? TicketFile { get; set; }

    }
}