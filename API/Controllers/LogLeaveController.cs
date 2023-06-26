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
    [Route("api/log-leaves")]
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

        

        [HttpGet("staffs/{staffId}")]
        public async Task<ActionResult<List<LogLeaveDTO>>> GetLogLeavesAsyncByStaffId(int staffId)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            return await _logLeaveService.GetLogLeavesByStaffId(staffId);
        }

        [HttpGet("{logLeaveId}/staffs/{staffId}", Name = "GetLogLeaveByStaffIdAndLogLeaveId")]
        public async Task<ActionResult<LogLeaveDTO>> GetLogLeaveByStaffIdAndLogLeaveId(int logLeaveId, int staffId)
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

        [HttpPost("staffs/{staffId}")]
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

            int leaveDays  = await _logLeaveService.LeaveDaysCalculation
                                                (logLeaveCreationDTO.LeaveStart,
                                                logLeaveCreationDTO.LeaveEnd);


            logLeaveCreationDTO.LeaveDays = leaveDays;
            logLeaveCreationDTO.LeaveHours = leaveDays * 8;

            if (!await _leaveDayDetailService.IsLeaveDayDetailValid(
                staffId, 
                logLeaveCreationDTO.LeaveTypeId, 
                leaveDays))
            {
                return BadRequest("Invalid Leave Days");
            }

            var returnLogLeave = await _logLeaveService.CreateLogLeave(staffId, logLeaveCreationDTO);


            if (!await _userInfoService.IsSaveChangeAsync())
            {
                return BadRequest("Error");

            }


            return CreatedAtRoute(
                "GetLogLeaveByStaffIdAndLogLeaveId",
                new
                {
                    staffId = returnLogLeave.StaffId,
                    logLeaveId = returnLogLeave.LeaveLogId
                },
                returnLogLeave
            );
        }

        
        [HttpPut("{logLeaveId}/staffs/{staffId}")]
        public async Task<ActionResult<LogLeaveDTO>> UpdateLogLeave(int staffId, int logLeaveId, LogLeaveUpdateDTO logLeaveUpdateDTO)
        {

            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if(!await _logLeaveService.IsLogLeaveExist(logLeaveId))
            {
                return NotFound();
            }

            if (!await _leaveDayDetailService.IsLeaveTypeDetailExist(staffId, logLeaveUpdateDTO.LeaveTypeId))
            {
                return BadRequest("Invalid Leave Type Id Of Staff");
            }

            logLeaveUpdateDTO.LeaveDays = (logLeaveUpdateDTO.LeaveEnd - logLeaveUpdateDTO.LeaveStart).TotalDays;
            logLeaveUpdateDTO.LeaveHours = (int?)(logLeaveUpdateDTO.LeaveDays * 8);

            if (!await _leaveDayDetailService.IsLeaveDayDetailValid(staffId, logLeaveUpdateDTO.LeaveTypeId ,(int)logLeaveUpdateDTO.LeaveDays))
            {
                return BadRequest("Invalid Leave Days");
            }

            await _logLeaveService.UpdateLogLeave(staffId, logLeaveId, logLeaveUpdateDTO);

            if (_leaveDayDetailService.IsApproved(logLeaveUpdateDTO.Status))
            {
                await _leaveDayDetailService.DecreaseDayLeft(staffId, logLeaveUpdateDTO.LeaveTypeId, (int)logLeaveUpdateDTO.LeaveDays);
            }

            await _context.SaveChangesAsync();

            if(!await _userInfoService.IsSaveChangeAsync())
            {
                return BadRequest();

            }
            return NoContent();
        }

        [HttpPatch("{logLeaveId}/staffs/{staffId}")]
        public async Task<ActionResult<LogLeaveDTO>> UpdateStatusLogLeave(
            int staffId, 
            int logLeaveId, 
            JsonPatchDocument<LogLeaveUpdateDTO> patchDocument)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if (!await _logLeaveService.IsLogLeaveExist(logLeaveId))
            {
                return NotFound();
            }

            var logLeave = await _logLeaveService.GetLogLeaveAsync(staffId, logLeaveId);

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

            if (!await _userInfoService.IsSaveChangeAsync())
            {
                return BadRequest();
            }

            if (_leaveDayDetailService.IsApproved(logLeavePath.Status))
            {
                await _leaveDayDetailService.DecreaseDayLeft(staffId, logLeavePath.LeaveTypeId, (int)logLeavePath.LeaveDays);
            }

            return NoContent();
        }
    }
}