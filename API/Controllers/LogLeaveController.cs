using API.DTOs.LogLeaveDTO;
using API.Entities;
using API.Services;
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
        private readonly UserInfoService _userInfoService;
        private readonly LogLeaveService _logLeaveService;
        private readonly LeaveDayDetailService _leaveDayDetailService;

        public LogLeaveController(
            SwpProjectContext context, 
            IMapper mapper,
            UserInfoService userInfoService,
            LogLeaveService logLeaveService,
            LeaveDayDetailService leaveDayDetailService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            _logLeaveService = logLeaveService ?? throw new ArgumentNullException(nameof(logLeaveService));
            _leaveDayDetailService = leaveDayDetailService ?? throw new ArgumentNullException(nameof(leaveDayDetailService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }
        [HttpGet]
        public async Task<ActionResult<List<LogLeaveDTO>>> GetLogLeavesAsync()
        {
            return await _logLeaveService.GetLogLeaveDTOs();
        }

        

        [HttpGet("staffId/{staffId}")]
        public async Task<ActionResult<List<LogLeaveDTO>>> GetLogLeavesAsyncByStaffId(int staffId)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            return await _logLeaveService.GetLogLeavesByStaffId(staffId);
        }

        

        [HttpGet("staffId/{staffId}/logLeaveId/{logLeaveId}", Name = "GetLogLeaveByStaffIdAndLogLeaveId")]
        public async Task<ActionResult<LogLeaveDTO>> GetLogLeaveByStaffIdAndLogLeaveId(int staffId, int logLeaveId)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if (await _logLeaveService.IsLogLeaveExist(logLeaveId))
            {
                return NotFound();
            }

            var returnLogLeave = await _logLeaveService.GetLogLeaveByStaffIdAndLogLeaveId(staffId, logLeaveId);

            return returnLogLeave;
        }

        

        [HttpPost]
        public async Task<ActionResult<LogLeaveDTO>> CreateLogLeaveByStaffId(int staffId, LogLeaveCreationDTO logLeaveCreationDTO)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if(!await _leaveDayDetailService.IsLeaveTypeDetailExist(staffId, logLeaveCreationDTO.LeaveTypeId))
            {
                return BadRequest("Invalid Leave Type Id Of Staff");
            }

            if(!await _leaveDayDetailService.IsLeaveDayDetailValid(staffId, (int)logLeaveCreationDTO.LeaveDays))
            {
                return BadRequest("Invalid Leave Days");
            }

            var user = await _logLeaveService.GetUserLogLeave(staffId);

            var logLeave = _mapper.Map<LogLeave>(logLeaveCreationDTO);

            if (logLeave == null)
            {
                return NotFound();
            }

            user.LogLeaves.Add(logLeave);

            await _context.SaveChangesAsync();

            return CreatedAtRoute(
                "GetLogLeaveByStaffIdAndLogLeaveId",
                new
                {
                    staffId = logLeave.StaffId,
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
                                         .Where(c => c.StaffId == staffId && c.LeaveLogId == logLeaveId)
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