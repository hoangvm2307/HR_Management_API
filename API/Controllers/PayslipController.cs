using API.DTOs;
using API.DTOs.PayslipDTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/manage-payslip")]
    public class PayslipController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public PayslipController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }


        [HttpGet]
        public async Task<ActionResult<List<Payslip>>> GetPayslipList()
        {
            var payslip = await _context.Payslips.ToListAsync();

            if(payslip == null)
            {
                return NotFound();
            }

            return payslip;
        }

        [HttpGet("StaffId")]
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

        // [HttpPost]
        // public async Task<ActionResult<PayslipDTO>> CreatePayslip(int StaffId)
        // {

        //     var userInfo = await _context.UserInfors.FirstOrDefaultAsync(c => c.StaffId == StaffId);
            
        //     if (userInfo == null)
        //     {
        //         return NotFound();
        //     }

        //     int grossSalary = 300000;

        //     int NoOfDependences = 0;
        //     string Type = "net";

        //     var payslipExtensions = new PayslipExtensions();

        //     PayslipDTO payslip = new PayslipDTO();

        //     if (Type.Equals("net"))
        //     {
        //         payslip = payslipExtensions.ConvertGrossToNet(grossSalary, NoOfDependences);
        //     }
        //     else
        //     {
        //         payslip = payslipExtensions.ConvertGrossToNet(grossSalary, NoOfDependences);
        //     }

        //     var finalPayslip = _mapper.Map<Entities.Payslip>(payslip);

        //     userInfo.Payslips.Add(finalPayslip);

        //     await _context.SaveChangesAsync();

        //     if (await _context.SaveChangesAsync() > 0)
        //     {
        //         return NoContent();
        //     }

        //     return NotFound();
        //     // return Ok(payslip);
        // }

    }
}