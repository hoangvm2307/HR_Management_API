using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using API.Extensions;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/payslips")]
    public class PayslipController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly PayslipService _payslipService;
        private readonly UserInfoService _userInfoService;
        private readonly PersonnelContractService _personnelContractService;
        private readonly LogLeaveService _logLeaveService;

        public PayslipController(
            SwpProjectContext context,
            IMapper mapper,
            PayslipService payslipService,
            UserInfoService userInfoService,
            PersonnelContractService personnelContractService,
            LogLeaveService logLeaveService
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _payslipService = payslipService ?? throw new ArgumentNullException(nameof(payslipService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
            _logLeaveService = logLeaveService ?? throw new ArgumentNullException(nameof(logLeaveService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }


        [HttpGet]
        public async Task<ActionResult<List<PayslipDTO>>> GetPayslips()
        {
            return await _payslipService.GetPayslipAsync();
        }

        

        [HttpGet("{staffId}")]
        public async Task<ActionResult<List<PayslipDTO>>> GetPayslipListByStaffId(int staffId)
        {
            return await _payslipService.GetPayslipOfStaff(staffId);
        }

        [HttpGet("{payslipId}/staffs/{staffId}")]
        public async Task<ActionResult<PayslipDTO>> GetPayslipByStaffId(int staffId, int payslipId)
        {
            if(!await _payslipService.IsPayslipExist(staffId, payslipId))
            {
                return NotFound();
            }

            return await _payslipService.GetPayslipOfStaffByPayslipId(staffId, payslipId);

        }

        [HttpPost("staffs/{staffId}")]
        public async Task<ActionResult<PayslipDTO>> CreatePayslipByStaffIdForAMonth(
            int staffId,
            int month,
            int year
            )
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if (!await _personnelContractService.IsValidContractExist(staffId))
            {
                return NotFound();
            }

            await _payslipService.AddPayslipToDatabase(staffId, month, year);


            return NoContent();
            ////Du lieu Fake

            //var personnelContractDTO = _mapper.Map<PersonnelContractDTO>(personnelContract);
            //var userInfo = await _userInfoService.GetUserInforEntityByStaffId(staffId);

            //var PayslipExtensions = new PayslipExtensions(_context, _mapper);

            //PayslipDTO payslipDTO = await PayslipExtensions.ConvertGrossToNet(personnelContractDTO, dateOnly.Month, dateOnly.Year);

            //var finalPayslips = _mapper.Map<Payslip>(payslipDTO);

            //var userInfoNew = await _context.UserInfors.Include(c => c.Payslips).Where(c => c.StaffId == staffId).FirstOrDefaultAsync();

            //userInfoNew.Payslips.Add(finalPayslips);

            //await _context.SaveChangesAsync();

            //return CreatedAtRoute(
            //    "GetPayslipListByStaffId",
            //    finalPayslips
            //);
            //return NoContent();

        }

    }
}