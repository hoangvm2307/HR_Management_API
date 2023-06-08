using API.DTOs.LogLeaveDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/log-leave")]
    public class LogLeaveController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public LogLeaveController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }
        [HttpGet]
        public async Task<ActionResult<List<LogLeaveDTO>>> GetLogLeavesAsync()
        {
            var logleaves = await _context.LogLeaves
                                    .Include(c => c.LeaveType)
                                    .ToListAsync();

            var returnLogLeaves = _mapper.Map<List<LogLeaveDTO>>(logleaves);

            return returnLogLeaves;
        }

        [HttpGet("staffId/{staffId}")]
        public async Task<ActionResult<List<LogLeaveDTO>>> GetLogLeavesAsyncByStaffId(int staffId)
        {
            var staffInfo = await _context.UserInfors.Where(c => c.StaffId == staffId).ToListAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }

            var logLeaves = await _context.LogLeaves
                                    .Include(c => c.LeaveType)
                                    .Where(c => c.StaffId == staffId)
                                    .ToListAsync();

            var returnLogLeaves = _mapper.Map<List<LogLeaveDTO>>(logLeaves);

            return returnLogLeaves;
        }

        [HttpGet("staffId/{staffId}/logLeaveId/{logLeaveId}")]
        public async Task<ActionResult<LogLeaveDTO>> GetLogLeavesAsyncById(int staffId, int logLeaveId)
        {
            var LogLeave = await _context.LogLeaves
                                        .Include(c => c.LeaveType)
                                        .Where(c => c.LeaveLogId == logLeaveId && c.LeaveLogId == logLeaveId)
                                        .FirstOrDefaultAsync();

            if (LogLeave == null)
            {
                return NotFound();
            }

            var reuturnLogLeave = _mapper.Map<LogLeaveDTO>(LogLeave);

            return reuturnLogLeave;
        }

        [HttpPost]
        public async Task<ActionResult<LogLeaveDTO>> CreateLogLeaveByStaffId(int staffId, LogLeaveCreationDTO logLeaveCreationDTO)
        {
            var staffInfo = await _context.UserInfors
                                            .Include(c => c.LogLeaves)
                                            .Where(c => c.StaffId == staffId)
                                            .FirstOrDefaultAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }

            var logLeave = _mapper.Map<LogLeave>(logLeaveCreationDTO);

            if (logLeave == null)
            {
                return NotFound();
            }

            staffInfo.LogLeaves.Add(logLeave);
            await _context.SaveChangesAsync();



            return CreatedAtRoute(
                new
                {
                    staffId = staffId,
                    logLeaveId = logLeave.LeaveLogId
                },
                logLeave
            );
        }

        [HttpPut]
        public async Task<ActionResult<LogLeaveDTO>> UpdateLogLeave(int staffId, int logLeaveId, LogLeaveUpdateDTO logLeaveUpdateDTO)
        {
            var logLeave = await _context.LogLeaves
                                         .Include(c => c.LeaveType)
                                         .Where(c => c.LeaveLogId == logLeaveId && c.LeaveLogId == logLeaveId)
                                         .FirstOrDefaultAsync();

            if (logLeave == null)
            {
                return NotFound();
            }

            var returnLogLeave = _mapper.Map(logLeaveUpdateDTO, logLeave);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult<LogLeaveDTO>> UpdateStatusLogLeave(int staffId, int logLeaveId, JsonPatchDocument<LogLeaveUpdateDTO> patchDocument)
        {
            var logLeave = await _context.LogLeaves
                                                     .Include(c => c.LeaveType)
                                                     .Where(c => c.LeaveLogId == logLeaveId && c.LeaveLogId == logLeaveId)
                                                     .FirstOrDefaultAsync();

            if (logLeave == null)
            {
                return NotFound();
            }

            var logLeavePath = _mapper.Map<LogLeaveUpdateDTO>(logLeave);

            patchDocument.ApplyTo(logLeavePath, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(logLeavePath))
            {
                return BadRequest();
            }

            _mapper.Map(logLeavePath, logLeave);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}