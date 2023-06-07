using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.TicketDTO;
using API.Entities;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TicketTypeController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public TicketTypeController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<ActionResult<TicketType>> CreateTicketType([FromBody] TicketTypeCreateDto ticketTypeDto)
        {
            if(ticketTypeDto == null) return BadRequest("Ticket Type data is missing");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var returnTicketType = _mapper.Map<TicketType>(ticketTypeDto);

            _context.TicketTypes.Add(returnTicketType);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetTicketType), new {id = returnTicketType.TicketTypeId}, returnTicketType);

            return BadRequest(new ProblemDetails {Title = "Problem Adding Ticket Type"});
        }

        [HttpPatch]
        public async Task<ActionResult<TicketType>> PatchTicketType(int id, [FromBody] JsonPatchDocument<TicketTypeUpdateDto> patchDocument)
        {
            if(patchDocument == null) return BadRequest("Problem with patch document");
            
            var ticketType = await _context.TicketTypes.FindAsync(id);

            if(ticketType == null) return NotFound();

            var ticketTypeDto = _mapper.Map<TicketTypeUpdateDto>(ticketType);

            patchDocument.ApplyTo(ticketTypeDto, ModelState);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            _mapper.Map(ticketTypeDto, ticketType);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetTicketType), new {id = ticketType.TicketTypeId}, ticketType);
            
            return BadRequest(new ProblemDetails {Title = "Problem Updating Ticket"});
        }

        [HttpPut]
        public async Task<ActionResult<TicketType>> PutTicketType(int id, [FromBody] TicketTypeUpdateDto updatedTicketType)
        {
            if(updatedTicketType == null) return BadRequest("Invalid Department Data");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingTicketType = await _context.TicketTypes.FindAsync(id);

            if(existingTicketType == null) return NotFound("Department Not Found");

            existingTicketType.TicketName = updatedTicketType.TicketName;

            _context.TicketTypes.Update(existingTicketType);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingTicketType);

            return BadRequest(new ProblemDetails {Title = "Problem Update TicketType"});
        }
    }
}