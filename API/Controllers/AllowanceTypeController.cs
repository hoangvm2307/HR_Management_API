using API.DTOs.PersonnelContractDTO;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/allowance-types")]
    [ApiController]
    public class AllowanceTypeController : ControllerBase
    {
        private readonly AllowanceTypeService _allowanceTypeService;

        public AllowanceTypeController(AllowanceTypeService allowanceTypeService)
        {
            _allowanceTypeService = allowanceTypeService ?? throw new ArgumentNullException(nameof(allowanceTypeService));
        }

        [HttpGet]
        public async Task<ActionResult<List<AllowanceTypeDTO>>> GetAllowanceTypes()
        {
            return await _allowanceTypeService.GetAllowanceTypeDTOsAsync();
        }

        [HttpGet("valid/contracts/{contractId}")]
        public async Task<ActionResult<List<AllowanceTypeDTO>>> GetValidAllowanceTypesOfContract(int contractId)
        {
            return await _allowanceTypeService.GetValidAllowanceTypes(contractId);
        }
    }
}
