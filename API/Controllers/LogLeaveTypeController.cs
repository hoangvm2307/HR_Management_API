using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.LeaveTypeDTO;
using API.DTOs.LogLeaveDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/leave-type")]
    public class LogLeaveTypeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SwpProjectContext _context;
        public LogLeaveTypeController(SwpProjectContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // [HttpGet]
        // public async Task<ActionResult<List<LeaveTypeWithLogLeavesDTO>>> GetLeaveTypesAsync()
        // {
        //     var leaveTypes = await _context.LeaveTypes.Include(c => c.LogLeaves).ToListAsync();

        //     var returnLeaveTypes = _mapper.Map<List<LeaveTypeWithLogLeavesDTO>>(leaveTypes);

        //     return returnLeaveTypes;
        // }
        [HttpGet]
        public async Task<ActionResult<List<LeaveTypeDTO>>> GetLeaveTypes()
        {
            var leaveTypes = await _context.LeaveTypes.ToListAsync();

            var returnLeaveTypes = _mapper.Map<List<LeaveTypeDTO>>(leaveTypes);

            return returnLeaveTypes;
        }

        [HttpGet("{leaveTypeId}")]
        public async Task<ActionResult<LeaveTypeDTO>> GetLeaveType(int leaveTypeId)
        {
            var leaveType = await _context.LeaveTypes.Where(c => c.LeaveTypeId == leaveTypeId).FirstOrDefaultAsync();

            if (leaveType == null)
            {
                return NotFound();
            }

            var returnLeaveType = _mapper.Map<LeaveTypeDTO>(leaveType);

            return returnLeaveType;
        }

        [HttpPost]
        public async Task<ActionResult<LeaveTypeDTO>> CreateLeaveType(LeaveTypeCreationDTO leaveTypeCreationDTO)
        {

            var leaveTypeCreation = _mapper.Map<LeaveType>(leaveTypeCreationDTO);

            _context.LeaveTypes.Add(leaveTypeCreation);
            await _context.SaveChangesAsync();

            
            if(!(await _context.SaveChangesAsync() >=0))
            {
                return NotFound();
            }

            return CreatedAtRoute(
                new { leaveTypeId = leaveTypeCreation.LeaveTypeId },
                leaveTypeCreation
            );
        }

        [HttpPut]
        public async Task<ActionResult<LeaveTypeDTO>> UpdateLeaveType(int leaveTypeId, LeaveTypeUpdateDTO leaveTypeUpdateDTO)
        {
            var leaveType = await _context.LeaveTypes.Where(c => c.LeaveTypeId == leaveTypeId).FirstOrDefaultAsync();

            if (leaveType == null)
            {
                return NotFound();
            }

            var returnLeaveType = _mapper.Map(leaveTypeUpdateDTO, leaveType);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}