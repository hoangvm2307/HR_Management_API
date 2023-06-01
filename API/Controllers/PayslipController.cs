using API.DTOs;
using API.DTOs.PayslipDTOs;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/payslip")]
    public class PayslipController : ControllerBase
    {
        [HttpGet]
        public ActionResult<PayslipDTO> Payslip(
            decimal grossSalary,
            int NoOfDependences = 0,
            string Type = "net"
            )
        {
            var payslipExtensions = new PayslipExtensions();
            var payslip = payslipExtensions.ConvertGrossToNet(grossSalary, NoOfDependences);
            
            return Ok(payslip);
        }
    }
}