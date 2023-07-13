using API.DTOs.LeaveDayDetailDTO;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  [Route("api/leave-day-detail")]
  [ApiController]
  public class LeaveDayDetailController : ControllerBase
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    private readonly PersonnelContractService _personnelContractService;
    private readonly UserInfoService _userInfoService;
    private readonly LeaveDayDetailService _leaveDayDetailService;

    public LeaveDayDetailController(
        SwpProjectContext context,
        IMapper mapper,
        PersonnelContractService personnelContractService,
        UserInfoService userInfoService,
        LeaveDayDetailService leaveDayDetailService)
    {
<<<<<<< HEAD
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
      _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
      _leaveDayDetailService = leaveDayDetailService ?? throw new ArgumentNullException(nameof(leaveDayDetailService));
      _context = context ?? throw new ArgumentNullException(nameof(context));
=======
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly PersonnelContractService _personnelContractService;
        private readonly UserInfoService _userInfoService;
        private readonly LeaveDayDetailService _leaveDayDetailService;

        public LeaveDayDetailController(
            SwpProjectContext context,
            IMapper mapper,
            PersonnelContractService personnelContractService,
            UserInfoService userInfoService,
            LeaveDayDetailService leaveDayDetailService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            _leaveDayDetailService = leaveDayDetailService ?? throw new ArgumentNullException(nameof(leaveDayDetailService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("{staffId}")]
        public async Task<ActionResult<List<LeaveDayDetailDTO>>> GetDetailDTOsAsync(int staffId)
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            return await _leaveDayDetailService.GetLeaveDayDetailDTOs(staffId);
        }

        [HttpPost("{staffId}")]
        public async Task<ActionResult<LeaveDayDetailDTO>> CreateLeaveDayDetailByStaffId(int staffId)
        {
            if(!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if(await _leaveDayDetailService.IsLeaveDayDetailExist(staffId))
            {
                return BadRequest("Exist ");
            }
            await _leaveDayDetailService.CreateLeaveDayDetail(staffId);


            return NoContent();
        }
>>>>>>> Linh09
    }

    [HttpGet("{staffId}")]
    public async Task<ActionResult<List<LeaveDayDetailDTO>>> GetDetailDTOsAsync(int staffId)
    {
      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }

      return await _leaveDayDetailService.GetLeaveDayDetailDTOs(staffId);
    }

    [HttpPost("{staffId}")]
    public async Task<ActionResult<LeaveDayDetailDTO>> CreateLeaveDayDetailByStaffId(int staffId)
    {
      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }
 
      if (await _leaveDayDetailService.IsLeaveDayDetailExist(staffId))
      {
        return BadRequest("Exist ");
      }
      await _leaveDayDetailService.CreateLeaveDayDetail(staffId);


      return NoContent();
    }
  }
}
