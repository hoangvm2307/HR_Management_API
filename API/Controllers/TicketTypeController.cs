using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TicketTypeController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        public TicketTypeController(SwpProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketType>>> GetTicketTypes()
        {
            var ticketTypes = await _context.TicketTypes
                .ToListAsync();

            return ticketTypes;
        }

        [HttpGet("{id}", Name ="GetTicketType")]
        public async Task<ActionResult<TicketType>> GetTicketType(int id)
        {
            var ticketType = await _context.TicketTypes
                .FirstOrDefaultAsync(t => t.TicketTypeId == id);
            return ticketType;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveTicketType(int id)
        {
            var ticketType = await _context.TicketTypes
                .FirstOrDefaultAsync(t => t.TicketTypeId == id);

            if(ticketType == null) return NotFound();

            _context.TicketTypes.Remove(ticketType);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails {Title = "Problem Removing TicketType"});
        }

        [HttpPost]
        public async Task<ActionResult<TicketType>> CreateTicketType([FromBody] TicketType ticketType)
        {
            if(ticketType == null) return BadRequest("Ticket Type data is missing");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            _context.TicketTypes.Add(ticketType);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetTicketType), new {id = ticketType.TicketTypeId}, ticketType);

            return BadRequest(new ProblemDetails {Title = "Problem Adding Ticket Type"});
        }

        [HttpPatch]
        public async Task<ActionResult<TicketType>> UpdateTicketType(int id, [FromBody] TicketType updatedTicketType)
        {
            if(updatedTicketType == null || updatedTicketType.TicketTypeId != id)
            {
                return BadRequest("Invalid TicketType data");
            }
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingTicketType = await _context.TicketTypes.FindAsync(id);

            if(existingTicketType == null) return NotFound("TicketType Not Found");

            existingTicketType.TicketName = updatedTicketType.TicketName;

            _context.TicketTypes.Update(existingTicketType);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingTicketType);

            return BadRequest(new ProblemDetails {Title = "Problem Update Department"});
        }
    }
}