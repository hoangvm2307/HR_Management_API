using API.DTOs.AllowanceDTO;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/allowance")]
    public class AllowanceController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public AllowanceController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        [HttpGet]
        public async Task<ActionResult<List<AllowanceDTO>>> GetAllowancesAsync()
        {
            var allowances = await _context.Allowances
                                .Include(c => c.AllowanceType)
                                .ToListAsync();

            var finalAllowances = _mapper.Map<List<AllowanceDTO>>(allowances);

            return finalAllowances;
        }


        [HttpGet("{contractId}")]
        public async Task<ActionResult<List<AllowanceDTO>>> GetAllowanceAsync(int contractId)
        {
            if (!await IsContractExist(contractId))
            {
                return NotFound();
            }

            var allowances = await _context.Allowances
                                .Include(c => c.AllowanceType)
                                .Where(c => c.ContractId == contractId).ToListAsync();

            var finalAllowances = _mapper.Map<List<AllowanceDTO>>(allowances);

            return finalAllowances;
        }

        [HttpPost]
        public async Task<ActionResult<AllowanceDTO>> CreateAllowanceAsync(int contractId, AllowanceCreationDTO allowanceCreationDTO)
        {
            if (!await IsContractExist(contractId))
            {
                return NotFound();
            }

            var allowance = _mapper.Map<Allowance>(allowanceCreationDTO);

            var allowanceFromStore = await _context.PersonnelContracts
                                .Include(c => c.Allowances)
                                .Where(c => c.ContractId == contractId)
                                .FirstOrDefaultAsync();

            if (allowanceFromStore == null)
            {
                return NotFound();
            }

            allowanceFromStore.Allowances.Add(allowance);
            await SaveChangeAsync();

            if (!await SaveChangeAsync())
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<AllowanceDTO>> UpdateAllowanceAsync(
            int contractId,
            int allowanceId,
            AllowanceUpdateDTO allowanceUpdateDTO
        )
        {
            if (!await IsContractExist(contractId))
            {
                return NotFound();
            }

            var allowance = await GetAllowanceByAllowanceId(contractId, allowanceId);

            if (allowance == null)
            {
                return NotFound();
            }

            var returnAllowance = _mapper.Map(allowanceUpdateDTO, allowance);

            await _context.SaveChangesAsync();

            if (!await SaveChangeAsync())
            {
                return NotFound();
            }

            return NoContent();
        }

        

        [HttpPatch]
        public async Task<ActionResult<AllowanceDTO>> PartiallUpdateAllowance(
            int contractId,
            int allowanceId,
            JsonPatchDocument<AllowanceUpdateDTO> patchDocument
        )
        {
            if(!await IsContractExist(contractId))
            {
                return NotFound();
            }

            var allowance = await GetAllowanceByAllowanceId(contractId, allowanceId);

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

            if(!await SaveChangeAsync())
            {
                return NotFound();
            }

            return NoContent();

        }

        private async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        private async Task<bool> IsContractExist(int contractId)
        {
            return await _context.PersonnelContracts.AnyAsync(c => c.ContractId == contractId);
        }
        private async Task<Allowance?> GetAllowanceByAllowanceId(int contractId, int allowanceId)
        {
            return await _context.Allowances.Where(c => c.ContractId == contractId && c.AllowanceId == allowanceId)
                                            .FirstOrDefaultAsync();
        }
    }
}