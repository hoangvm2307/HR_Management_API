using API.DTOs.LogOtDTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/logot")]
    public class LogOtController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public LogOtController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        [HttpGet]
        public async Task<ActionResult<List<LogOtDTO>>> GetLogOtList()
        {
            var logOtList = await _context.LogOts
                                .ToListAsync();

            var returnLogOtList = _mapper.Map<List<LogOtDTO>>(logOtList);

            return returnLogOtList;
        }
        [HttpGet("{StaffId}", Name = "GetLogOtListByStaffId")]
        public async Task<ActionResult<List<LogOtDTO>>> GetLogOtListByStaffId(int StaffId)
        {
            var logOtList = await _context.LogOts
                                .Where(c => c.StaffId == StaffId)
                                .ToListAsync();

            var returnLogOtList = _mapper.Map<List<LogOtDTO>>(logOtList);

            return returnLogOtList;
        }

        [HttpPost]
        public async Task<ActionResult<LogOtDTO>> CreateLogOtByStaffId(int StaffId, LogOtCreationDTO createLogOtDTO)
        {
            var staffInfo = await _context.UserInfors
                                .Include(c => c.LogOts).Where(c => c.StaffId == StaffId)
                                .FirstOrDefaultAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }

            var logOt = _mapper.Map<LogOt>(createLogOtDTO);

            staffInfo.LogOts.Add(logOt);

            await _context.SaveChangesAsync();

            if (!(await _context.SaveChangesAsync() >= 0))
            {
                return NotFound();
            }

            var returnLogOtDTO = _mapper.Map<LogOtDTO>(logOt);

            return CreatedAtRoute(
                "GetLogOtListByStaffId",
                new { StaffId = StaffId },
                returnLogOtDTO
            );
        }
        
        [HttpPut]
        public async Task<ActionResult<LogOtDTO>> UpdateLogOt(int StaffId, int LogOtId, LogOtUpdateDTO logOtUpdateDTO)
        {
            var staffInfo = await _context.UserInfors.Include(c => c.LogOts).Where(c => c.StaffId == StaffId).FirstOrDefaultAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }


            var logOt = await _context.LogOts.Where(c => c.StaffId == StaffId && c.OtLogId == LogOtId).FirstOrDefaultAsync();

            if (logOt == null)
            {
                return NotFound();
            }

            _mapper.Map(logOtUpdateDTO, logOt);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
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

            _context.LogOts.Remove(logOt);

            await   _context.SaveChangesAsync();

            return NoContent();
        }
    }
}