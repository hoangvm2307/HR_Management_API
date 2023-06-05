using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/contract")]
    public class ContractController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public ContractController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        [HttpGet]
        public async Task<ActionResult<List<PersonnelContractDTO>>> GetPersonnelContracts()
        {


            var PersonnelContracts = await _context.PersonnelContracts
                                            .Include(c => c.ContractType)
                                            .Include(c => c.Allowances)
                                            .ThenInclude(c => c.AllowanceType)
                                            .ToListAsync();

            var finalPersonnelContracts = _mapper.Map<List<PersonnelContractDTO>>(PersonnelContracts);

            return finalPersonnelContracts;
        }

        [HttpGet("staffId", Name = "GetPersonnelContractByStaffId")]
        public async Task<ActionResult<PersonnelContractDTO>> GetPersonnelContractByStaffId(int staffId)
        {

            if (!await IsStaffInfoExist(staffId))
            {
                return NotFound();
            }

            var PersonnelContract = await _context.PersonnelContracts
                                        .Include(c => c.ContractType)
                                        .Include(c => c.Allowances)
                                        .ThenInclude(c => c.AllowanceType)
                                        .Where(c => c.StaffId == staffId)
                                        .FirstOrDefaultAsync();


            if (PersonnelContract == null)
            {
                return NotFound();
            }

            var finalPersonnelContracts = _mapper.Map<PersonnelContractDTO>(PersonnelContract);

            return finalPersonnelContracts;
        }

        [HttpPost]
        public async Task<ActionResult<PersonnelContractDTO>> CreatePersonnelContract(int staffId, PersonnelContractCreationDTO personnelContractCreationDTO)
        {
            if (!await IsStaffInfoExist(staffId))
            {
                return NotFound();
            }

            var PersonnelContract = _mapper.Map<PersonnelContract>(personnelContractCreationDTO);

            var userInfor = await _context.UserInfors.Include(c => c.PersonnelContracts).Where(c => c.StaffId == staffId).FirstOrDefaultAsync();

            if (userInfor == null)
            {
                return NotFound();
            }

            userInfor.PersonnelContracts.Add(PersonnelContract);
            await _context.SaveChangesAsync();

            if(!await SaveChangeAsync())
            {
                return NotFound();
            }

            var returnPersonnelContract = _mapper.Map<PersonnelContractDTO>(PersonnelContract);

            return CreatedAtRoute(
                new {staffId = staffId},
                returnPersonnelContract
            );
        }

        [HttpPut]
        public async Task<ActionResult<PersonnelContractDTO>> UpdatePersonnelContract(int staffId, int contractId, PersonnelContractUpdateDTO personnelContractUpdateDTO)
        {
            if (!await IsStaffInfoExist(staffId))
            {
                return NotFound();
            }

            var personnelContract = await _context.PersonnelContracts
                                            .Where(c => c.StaffId == staffId && c.ContractId == contractId)
                                            .FirstOrDefaultAsync();

            if (personnelContract == null)
            {
                return NotFound();
            }

            _mapper.Map(personnelContractUpdateDTO, personnelContract);

            await _context.SaveChangesAsync();

            if (!await SaveChangeAsync())
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult<PersonnelContractDTO>> PartiallyPersonnelContract(
                int staffId, 
                int contractId, 
                JsonPatchDocument<PersonnelContractUpdateDTO> patchDocument
                )
        {
            if(!await IsStaffInfoExist(staffId))
            {
                return NotFound();
            }

            var contractFromStore = await _context
                                        .PersonnelContracts
                                        .Where(c => c.StaffId == staffId && c.ContractId == contractId)
                                        .FirstOrDefaultAsync();

            if (contractFromStore == null)
            {
                return NotFound();
            }

            var contractPath = _mapper.Map<PersonnelContractUpdateDTO>(contractFromStore);

            patchDocument.ApplyTo(contractPath, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(contractPath))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(contractPath, contractFromStore);

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

        private async Task<bool> IsStaffInfoExist(int staffId)
        {
            return await _context.UserInfors.AnyAsync(c => c.StaffId == staffId);
        }
    }
}