using API.DTOs.AllowanceDTO;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/allowances")]
    public class AllowanceController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly AllowanceService _allowanceService;

        public AllowanceController(
            SwpProjectContext context,
            IMapper mapper,
            AllowanceService allowanceService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _allowanceService = allowanceService ?? throw new ArgumentNullException(nameof(allowanceService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        [HttpGet]
        public async Task<ActionResult<List<AllowanceDTO>>> GetAllowancesAsync()
        {
            var finalAllowances = await _allowanceService.GetAllowanceDTOs();

            return finalAllowances;
        }



        [HttpGet("contracts/{contractId}")]
        public async Task<ActionResult<List<AllowanceDTO>>> GetAllowanceAsync(int contractId)
        {
            if (!await _allowanceService.IsContractExist(contractId))
            {
                return NotFound();
            }

            var finalAllowances = await _allowanceService.GetAllowanceByContractId(contractId);

            return finalAllowances;
        }

        [HttpGet("{allowanceId}/contracts/{contractId}", Name = "GetAllowance")]
        public async Task<ActionResult<AllowanceDTO>> GetAllowanceByContractIdAndAllowanceId(int contractId, int allowanceId)
        {
            if (!await _allowanceService.IsContractExist(contractId))
            {
                return NotFound();
            }

            var finalAllowances = await _allowanceService.GetAllowanceByAllowanceId(contractId, allowanceId);

            return finalAllowances;
        }




        [HttpPost("contracts/{contractId}")]
        public async Task<ActionResult<AllowanceDTO>> CreateAllowanceAsync(
            int contractId,
            AllowanceCreationDTO allowanceCreationDTO)
        {
            if (!await _allowanceService.IsContractExist(contractId))
            {
                return NotFound();
            }

            if (!await _allowanceService.IsAllowanceTypeValid(allowanceCreationDTO.AllowanceTypeId))
            {
                return BadRequest("Invalid Allowance Type");
            }

            if (!await _allowanceService.IsProjectAlreadyHaveAllowanceType(contractId, allowanceCreationDTO.AllowanceTypeId))
            {
                return BadRequest("Already have this allowance ");
            }

            var returnAllowance = await _allowanceService.CreateAllowance(contractId, allowanceCreationDTO);

            if (!await _allowanceService.SaveChangeAsync())
            {
                return BadRequest("Save Change Async occur");
            }

            return CreatedAtRoute(
                "GetAllowance",
                new { contractId = returnAllowance.ContractId, allowanceId = returnAllowance.AllowanceId },
                returnAllowance
            );
        }




        [HttpPut("{allowanceId}/contracts/{contractId}")]
        public async Task<ActionResult<AllowanceDTO>> UpdateAllowanceAsync(
            int contractId,
            int allowanceId,
            AllowanceUpdateDTO allowanceUpdateDTO
        )
        {
            if (!await _allowanceService.IsContractExist(contractId))
            {
                return NotFound();
            }


            if (!await _allowanceService.IsAllowanceTypeValid(allowanceUpdateDTO.AllowanceTypeId))
            {
                return BadRequest("Invalid Allowance Type");
            }

            await _allowanceService.UpdateAllowance(contractId, allowanceId, allowanceUpdateDTO); 

            if (!await _allowanceService.SaveChangeAsync())
            {
                return BadRequest("Save Change Async Problem");
            }

            return NoContent();
        }

        

        [HttpPatch("{allowanceId}/contracts/{contractId}")]
        public async Task<ActionResult<AllowanceDTO>> PartiallUpdateAllowance(
            int contractId,
            int allowanceId,
            JsonPatchDocument<AllowanceUpdateDTO> patchDocument
        )
        {
            if (!await _allowanceService.IsContractExist(contractId))
            {
                return NotFound();
            }

            var allowance = await _allowanceService.GetAllowanceByAllowanceId(contractId, allowanceId);

            if(allowance == null)
            {
                return NotFound();
            }

            var allowancePath = _mapper.Map<AllowanceUpdateDTO>(allowance);

            patchDocument.ApplyTo(allowancePath, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(allowancePath))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(allowancePath, allowance);
            await _context.SaveChangesAsync();

            if(!await _allowanceService.SaveChangeAsync())
            {
                return NotFound();
            }

            return NoContent();

        }

        
    }
}