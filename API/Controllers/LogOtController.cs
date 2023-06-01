using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.LogOtDTOs;
using API.Entities;
using AutoMapper;
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
        public async Task<ActionResult<LogOtDTO>> CreateLogOtByStaffId(CreateLogOtDTO createLogOtDTO)
        {
            var staffInfo = await _context.LogOts
                                        .Where(c => c.StaffId == createLogOtDTO.StaffId)
                                        .FirstOrDefaultAsync();

            if (staffInfo == null)
            {
                return NotFound();
            }

            var logOt = _mapper.Map<LogOt>(createLogOtDTO);

            _context.LogOts.Add(logOt);

            await _context.SaveChangesAsync();

            if (!(await _context.SaveChangesAsync() >= 0))
            {
                return NotFound();
            }

            var returnLogOtDTO = _mapper.Map<LogOtDTO>(logOt);

            return CreatedAtRoute(
                "GetLogOtListByStaffId",
                new { StaffId = createLogOtDTO.StaffId },
                returnLogOtDTO
            );
        }
    }
}