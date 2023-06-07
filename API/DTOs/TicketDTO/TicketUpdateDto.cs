using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.TicketDTO
{
    public class TicketUpdateDto
    {
        public int? TicketTypeId { get; set; }

        public string TicketTitle { get; set; } = null!;

        public string? TicketFile { get; set; }
    }
}