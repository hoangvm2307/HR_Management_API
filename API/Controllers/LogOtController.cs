using API.DTOs.LogOtDTOs;
using API.DTOs.UserInforDTO;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/logots")]
    public class LogOtController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly LogOtService _logOtService;
        private readonly UserInfoService _userInfoService;

        public TheCalendarService TheCalendarService { get; set; }
        public LogOtController(
            SwpProjectContext context, 
            IMapper mapper, 
            LogOtService logOtService,
            UserInfoService userInfoService
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logOtService = logOtService ?? throw new ArgumentNullException(nameof(logOtService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            TheCalendarService = new TheCalendarService(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<LogOtDTO>>> GetLogOtList()
        {
            var returnLogOtList = await _logOtService.GetLogOts();

            return returnLogOtList;
        }

        [HttpGet("staffs/{staffId}", Name = "GetLogOtOfStaff")]
        public async Task<ActionResult<List<LogOtDTO>>> GetLogOtListByStaffId(int staffId)
        {
            if(!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            var logOtList = await _logOtService.GetLogOtByStaffIdAsync(staffId);

            return logOtList;
        }

        [HttpGet("{logOtId}", Name = "GetLogOtByOtId")]
        public async Task<ActionResult<LogOtDTO>> GetLogOTByOtId(int logOtId)
        {
            var logOt = await _logOtService.GetLogOtById(logOtId);

            if (logOt == null)
            {
                return NotFound();
            }

            return logOt;
        }


        [HttpPost("staffs/{staffId}")]
        public async Task<ActionResult<LogOtDTO>> CreateLogOtByStaffId(int staffId, LogOtCreationDTO createLogOtDTO)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            //Here 
            //if (!await _logOtService.IsDateTimeValid(createLogOtDTO.LogStart, createLogOtDTO.LogEnd))
            //{
            //    return BadRequest("Invalid DateTime");
            //}

            if (!await _logOtService.IsContainHoliday(createLogOtDTO.LogStart, createLogOtDTO.LogEnd))
            {
                return BadRequest("Not contain holiday");
            }
            var logOt = _mapper.Map<LogOt>(createLogOtDTO);


            if (await _logOtService.IsDuplicateLogOt(staffId, logOt.OtLogId, createLogOtDTO.LogStart, createLogOtDTO.LogEnd))
            {
                return BadRequest("You have signed up for overtime");
            }

            await _logOtService.CreateLogOT(staffId, createLogOtDTO);

            if (!(await _context.SaveChangesAsync() >= 0))
            {
                return BadRequest("Save Change Problem");
            }

            var returnLogOtDTO = _mapper.Map<LogOtDTO>(logOt);

            return CreatedAtRoute(
                "GetLogOtOfStaff",
                new
                {
                    staffId = returnLogOtDTO.StaffId
                },
                 returnLogOtDTO
            );
        }


        [HttpPut("{logOtId}/staffs/{staffId}")]
        public async Task<ActionResult<LogOtDTO>> UpdateLogOt(int staffId, int logOtId, LogOtUpdateDTO logOtUpdateDTO)
        {
            var staffInfo = await _context.UserInfors.Include(c => c.LogOts).Where(c => c.StaffId == staffId).FirstOrDefaultAsync();

            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if (!await _logOtService.IsLogOtExist(logOtId))
            {
                return NotFound();
            }

            await _logOtService.UpdateLogOt(staffId, logOtId,logOtUpdateDTO);

            return NoContent();
        }

        

        [HttpPatch("/{logOtId}/staffs/{staffId}")]
        public async Task<ActionResult<LogOtDTO>> PartiallyUpdateLogOt(
            int StaffId,
            int LogOtId,
            JsonPatchDocument<LogOtUpdateDTO> patchDocument
        )
        {
            var staffInfo = await _context.UserInfors.Where(c => c.StaffId == StaffId).FirstOrDefaultAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }
            var LogOtFromStore = await _context.LogOts.Where(c => c.StaffId == StaffId && c.OtLogId == LogOtId).FirstOrDefaultAsync();

            if (LogOtFromStore == null)
            {
                return NotFound();
            }

            var logOtpath = _mapper.Map<LogOtUpdateDTO>(LogOtFromStore);

            patchDocument.ApplyTo(logOtpath,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(logOtpath))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(logOtpath, LogOtFromStore);
            await _context.SaveChangesAsync();

            return NoContent();   
        }

        [HttpDelete]
        public async Task<ActionResult<LogOtDTO>> DeleteLogOt(int StaffId, int LogOtId)
        {
            var staffInfo = await _context.UserInfors.Where(c => c.StaffId == StaffId).FirstOrDefaultAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }

            var logOt = await _context.LogOts.Where(c => c.StaffId == StaffId && c.OtLogId == LogOtId).FirstOrDefaultAsync();

            if (logOt == null)
            {
                return NotFound();
            }

            //_context.LogOts.Remove(logOt);

            //await   _context.SaveChangesAsync();

            return NoContent();
        }


    }
}