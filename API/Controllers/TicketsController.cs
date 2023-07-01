using System.Security.Claims;
using API.DTOs.TicketDTO;
using API.DTOs.UserDTO;
using API.DTOs.UserInforDTO;
using API.Entities;
using API.Extensions;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class TicketsController : BaseApiController
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly AccountController _accountController;
    private readonly UserService _userService;
    public TicketsController(SwpProjectContext context, IMapper mapper,
    UserManager<User> userManager,
    AccountController accountController, UserService userService)
    {
      _userService = userService;
      _accountController = accountController;
      _userManager = userManager;
      _mapper = mapper;
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetTickets()
    {
      var ticketDtos = await _context.Tickets
          .Include(t => t.TicketType)
          .Select(t => new TicketDto
          {
            TicketId = t.TicketId,
            StaffId = t.StaffId,
            StaffName = t.Staff.LastName + " " + t.Staff.FirstName, // Include the staff name directly in the projection
            TicketTypeId = t.TicketTypeId,
            TicketName = t.TicketType.TicketName,
            TicketReason = t.TicketReason,
            TicketFile = t.TicketFile,
            TicketStatus = t.TicketStatus,
            ProcessNote = t.ProcessNote,
            RespondencesId = t.RespondencesId,
            Enable = t.Enable,
            CreateAt = t.CreateAt,
            ChangeStatusTime = t.ChangeStatusTime
          })
          .ToListAsync();

      return ticketDtos;
    }
    [HttpGet("currentusertickets")]
    public async Task<ActionResult<List<TicketDto>>> GetCurrentUserTickets()
    {
      var userInfor = await _userService.GetCurrentUserInfor(User);

      var ticketDtos = await _context.Tickets
          .Include(t => t.TicketType)
          .Where(t => t.StaffId == userInfor.StaffId) // Filter tickets by the current user's ID
          .Select(t => new TicketDto
          {
            TicketId = t.TicketId,
            StaffId = t.StaffId,
            StaffName = t.Staff.LastName + " " + t.Staff.FirstName,
            TicketTypeId = t.TicketTypeId,
            TicketName = t.TicketType.TicketName,
            TicketReason = t.TicketReason,
            TicketFile = t.TicketFile,
            TicketStatus = t.TicketStatus,
            Enable = t.Enable,
            ProcessNote = t.ProcessNote,
            RespondencesId = t.RespondencesId,
            ResponsdenceName = _context.UserInfors
              .Where(c => c.StaffId == t.RespondencesId)
              .Select(s => s.LastName + " " + s.FirstName)
              .FirstOrDefault(),
            CreateAt = t.CreateAt,
            ChangeStatusTime = t.ChangeStatusTime
          })
          .ToListAsync();

      return ticketDtos;
    }
    [HttpGet("otheruserstickets")]
    public async Task<ActionResult<List<TicketDto>>> GetOtherUsersTickets()
    {
      var userInfor = await _userService.GetCurrentUserInfor(User);

      var ticketDtos = await _context.Tickets
          .Include(t => t.TicketType)
          .Where(t => t.StaffId != userInfor.StaffId && t.Enable == true) // Filter tickets by the current user's ID
          .Select(t => new TicketDto
          {
            TicketId = t.TicketId,
            StaffId = t.StaffId,
            StaffName = t.Staff.LastName + " " + t.Staff.FirstName,
            TicketTypeId = t.TicketTypeId,
            TicketName = t.TicketType.TicketName,
            TicketReason = t.TicketReason,
            TicketFile = t.TicketFile,
            TicketStatus = t.TicketStatus,
            Enable = t.Enable,
            ProcessNote = t.ProcessNote,
            RespondencesId = t.RespondencesId,
            ResponsdenceName = _context.UserInfors
            .Where(c => c.StaffId == t.RespondencesId)
            .Select(s => s.LastName + " " + s.FirstName)
            .FirstOrDefault(),
            CreateAt = t.CreateAt,
            ChangeStatusTime = t.ChangeStatusTime
          })
          .ToListAsync();

      return ticketDtos;
    }


    [HttpGet("{id}", Name = "GetTicket")]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
      var ticket = await _context.Tickets
          .ProjectTicketToTicketDto()
          .FirstOrDefaultAsync(d => d.StaffId == id);
      return ticket;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveTicket(int id)
    {
      var ticket = await _context.Tickets
          .FirstOrDefaultAsync(t => t.TicketId == id);

      if (ticket == null) return NotFound();

      var userInfor = await _userService.GetCurrentUserInfor(User);

      if (userInfor == null) return BadRequest("User Not Found");

      ticket.TicketStatus = "Đã hủy";
      ticket.ChangeStatusTime = DateTime.Now;
      ticket.Enable = false;

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok();

      return BadRequest(new ProblemDetails { Title = "Problem removing Ticket" });
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateTicket(TicketCreateDto ticketDto)
    {
      if (ticketDto == null) return BadRequest("Department data is missing");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var userInfor = await _userService.GetCurrentUserInfor(User);

      if (userInfor == null) return BadRequest("No User Found");

      var ticket = new Ticket
      {
        StaffId = userInfor.StaffId,
        TicketTypeId = ticketDto.TicketTypeId,
        TicketReason = ticketDto.TicketReason,
        TicketFile = ticketDto.TicketFile,
        TicketStatus = "Chờ duyệt",
        Enable = true,
        CreateAt = DateTime.Now,
      };

      _context.Tickets.Add(ticket);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticket);

      return BadRequest(new ProblemDetails { Title = "Problem adding item" });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTicket(int id, [FromBody] JsonPatchDocument<TicketUpdateDto> patchDocument)
    {
      var ticket = await _context.Tickets.FindAsync(id);

      if (ticket == null) return NotFound();

      var ticketDto = _mapper.Map<TicketUpdateDto>(ticket);

      patchDocument.ApplyTo(ticketDto, ModelState);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      _mapper.Map(ticketDto, ticket);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return NoContent();

      return BadRequest("Problem Updating Ticket");
    }

    // For HR Staff
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTicketStatus(int id, [FromBody] TicketStatusDto ticketStatusDto)
    {
      if (ticketStatusDto == null) return BadRequest("Invalid ticket status data");

      var ticket = await _context.Tickets.FindAsync(id);

      if (ticket == null) return NotFound("Ticket not found");

      // Get the current approver
      var userInfor = await _userService.GetCurrentUserInfor(User);

      // Update the ticket status
      ticket.TicketStatus = ticketStatusDto.TicketStatus;
      ticket.ChangeStatusTime = DateTime.Now;
      ticket.RespondencesId = userInfor.StaffId;
      ticket.Enable = false;

      if (!string.IsNullOrWhiteSpace(ticketStatusDto.ProcessNote))
        ticket.ProcessNote = ticketStatusDto.ProcessNote;

      await _context.SaveChangesAsync();

      return NoContent();
    }
    //For current user
    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateTicket(int id, TicketUpdateDto ticketDto)
    {
      if (ticketDto == null) return BadRequest("Invalid ticket status data");

      var ticket = await _context.Tickets.FindAsync(id);

      if (ticket == null) return NotFound("Ticket not found");

      // Update the ticket status
      ticket.TicketTypeId = ticketDto.TicketTypeId;
      ticket.TicketReason = ticketDto.TicketReason;
      ticket.ChangeStatusTime = DateTime.Now;


      await _context.SaveChangesAsync();

      return NoContent();
    }

  }
}