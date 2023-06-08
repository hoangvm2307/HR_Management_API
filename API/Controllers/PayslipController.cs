using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/payslip")]
    public class PayslipController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public PayslipController(
            SwpProjectContext context,
            IMapper mapper
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }


        [HttpGet]
        public async Task<ActionResult<List<PayslipDTO>>> GetPayslipList()
        {
            var payslips = await _context.Payslips.ToListAsync();

            if (payslips == null)
            {
                return NotFound();
            }

            var payslipsDTO = _mapper.Map<List<PayslipDTO>>(payslips);

            return payslipsDTO;
        }

        [HttpGet("StaffId", Name = "GetPayslipListByStaffId")]
        public async Task<ActionResult<List<PayslipDTO>>> GetPayslipListByStaffId(int StaffId)
        {
            var payslips = await _context.Payslips
                .Where(c => c.StaffId == StaffId)
                .ToListAsync();

            if (payslips == null)
            {
                return NotFound();
            }

            var finalPayslips = _mapper.Map<List<PayslipDTO>>(payslips);

            return finalPayslips;
        }

        [HttpPost]
        public async Task<ActionResult<PayslipDTO>> CreatePayslipByStaffIdForAMonth(int StaffId, int month, int year)
        {

            var userInfo = await _context.UserInfors
                .Where(c => c.StaffId == StaffId && c.AccountStatus == true)
                .FirstOrDefaultAsync();

            if (userInfo == null)
            {
                return NotFound();
            }

            //Du lieu Fake

            var personnelContract = await _context.PersonnelContracts.Where(c => c.StaffId == StaffId).FirstOrDefaultAsync();

            if (personnelContract == null)
            {
                return NotFound();
            }

            var personnelContractDTO = _mapper.Map<PersonnelContractDTO>(personnelContract);

            // int GrossSalary = 30000000;

            // string type = "GrossToNet";

            var PayslipExtensions = new PayslipExtensions(_context, _mapper);

            PayslipDTO payslipDTO = await PayslipExtensions.ConvertGrossToNet(personnelContractDTO, month, year);

            var finalPayslips = _mapper.Map<Payslip>(payslipDTO);

            var userInfoNew = await _context.UserInfors.Include(c => c.Payslips).Where(c => c.StaffId == StaffId).FirstOrDefaultAsync();

            userInfoNew.Payslips.Add(finalPayslips);

            // _context.Payslips.Add(finalPayslips);

            await _context.SaveChangesAsync();

            // return CreatedAtRoute(
            //     "GetPayslipListByStaffId",
            //     finalPayslips
            // );
            return NoContent();

        }

    }
}