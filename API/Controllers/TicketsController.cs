using API.DTOs.TicketDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TicketsController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public TicketsController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketDto>>> GetTickets()
        {
            var tickets = await _context.Tickets
                .ProjectTicketToTicketDto()
                .ToListAsync();

            return tickets;
        }

        [HttpGet("{id}", Name="GetTicket")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .ProjectTicketToTicketDto()
                .FirstOrDefaultAsync(d => d.StaffId == id);
            return ticket;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveTicket(int id)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketId == id);
            
            if(ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails {Title = "Problem removing Ticket"});
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            if(ticket == null) return BadRequest("Department data is missing");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _context.Tickets.Add(ticket);
            
            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetTicket), new {id = ticket.TicketId});

            return BadRequest(new ProblemDetails {Title = "Problem adding item"});
        }

        [HttpPatch]
        public async Task<ActionResult<Ticket>> UpdateTicket(int id, [FromBody] Ticket updatedTicket)
        {
            if(updatedTicket == null || updatedTicket.TicketId != id)
            {
                return BadRequest("Invalid Ticket    Data");
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingTicket = await _context.Tickets.FindAsync(id);

            if(existingTicket == null) return NotFound("Ticket Not Found");

            existingTicket.TicketStatus = updatedTicket.TicketStatus;

            _context.Tickets.Update(existingTicket);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingTicket);

            return BadRequest(new ProblemDetails {Title = "Problem Update Ticket"});
        }
    }
}   