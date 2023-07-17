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
  [Route("api/contracts")]
  public class ContractController : ControllerBase
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    private readonly PersonnelContractService _personnelContractService;
    private readonly UserInfoService _userInfoService;

    public ContractController(
        SwpProjectContext context,
        IMapper mapper,
        PersonnelContractService personnelContractService,
        UserInfoService userInfoService)
    {
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
      _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
      _context = context ?? throw new ArgumentNullException(nameof(context));

    }

    [HttpGet]
    public async Task<ActionResult<List<PersonnelContractDTO>>> GetPersonnelContracts()
    {
      var personnelContracts = await _personnelContractService.GetPersonnelContractsAsync();

      return personnelContracts;
    }



    [HttpGet("{staffId}", Name = "GetPersonnelContractByStaffId")]
    public async Task<ActionResult<List<PersonnelContractDTO>>> GetPersonnelContractByStaffId(int staffId)
    {

      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }
      var finalPersonnelContract = await _personnelContractService
          .GetPersonnelContractById(staffId);

      return finalPersonnelContract;
    }

    [HttpGet("valid/{staffId}")]
    public async Task<ActionResult<PersonnelContractDTO>> GetValidPersonnelContractByStaffId(int staffId)
    {
      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }

      var validPersonnelContract = await _personnelContractService
          .GetValidPersonnelContractByStaffId(staffId);

      if (validPersonnelContract == null)
      {
        return NotFound();
      }

      return validPersonnelContract;
    }

    [HttpGet("{contractId}/staffs/{staffId}", Name = "GetContractByIdAndStaffId")]
    public async Task<ActionResult<PersonnelContractDTO>> GetContractByIdAndStaffId(int contractId, int staffId)
    {
      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }

      if (!await _personnelContractService.IsValidContractExist(contractId))
      {
        return NotFound();
      }

      var contract = await _personnelContractService.GetContractByIdAndStaffId(contractId, staffId);

      return contract;
    }

    [HttpPost("staffs/{staffId}")]
    public async Task<ActionResult<PersonnelContractDTO>> CreatePersonnelContract(
        int staffId,
        PersonnelContractCreationDTO personnelContractCreationDTO)
    {
      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }

      if (!await _personnelContractService
          .IsContractTypeValid(personnelContractCreationDTO.ContractTypeId))
      {
        return BadRequest("Invalid ContractTypeId");
      }

      //if (!await _personnelContractService.IsContractTimeValid(
      //    personnelContractCreationDTO.StartDate, 
      //    personnelContractCreationDTO.EndDate))
      //{
      //    return BadRequest("InValid DateTime");
      //}

      if (await _personnelContractService.IsValidContractExist(staffId))
      {
        return BadRequest("This account already have contract");
      }

      var returnPersonnelContract = await _personnelContractService
          .CreatePersonnelContract(staffId, personnelContractCreationDTO);


      if (!await SaveChangeAsync())
      {
        return NotFound();
      }


      return CreatedAtRoute(
          "GetContractByIdAndStaffId",
          new
          {
            contractId = returnPersonnelContract.ContractId,
            staffId = returnPersonnelContract.StaffId
          },
          returnPersonnelContract
      );
    }

    [HttpPut("{contractId}/staffs/{staffId}")]
    public async Task<ActionResult<PersonnelContractDTO>> UpdatePersonnelContract(int staffId, int contractId, PersonnelContractUpdateDTO personnelContractUpdateDTO)
    {
      if (!await _userInfoService.IsUserExist(staffId))
      {
        return NotFound();
      }

      if (!await _personnelContractService.IsContractTypeValid(personnelContractUpdateDTO.ContractTypeId))
      {
        return BadRequest("Invalid ContractTypeId");
      }

      if (!await _personnelContractService.IsContractTimeValid(personnelContractUpdateDTO.StartDate, personnelContractUpdateDTO.EndDate))
      {
        return BadRequest("InValid DateTime");
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

    [HttpPatch("{contractId}/staffs/{staffId}")]
    public async Task<ActionResult<PersonnelContractDTO>> PartiallyPersonnelContract(
            int staffId,
            int contractId,
            JsonPatchDocument<PersonnelContractUpdateDTO> patchDocument
            )
    {
      if (!await _userInfoService.IsUserExist(staffId))
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

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (!TryValidateModel(contractPath))
      {
        return BadRequest(ModelState);
      }

      _mapper.Map(contractPath, contractFromStore);

      await _context.SaveChangesAsync();

      if (!await SaveChangeAsync())
      {
        return NotFound();
      }

      return NoContent();
    }


    private async Task<bool> SaveChangeAsync()
    {
      return await _context.SaveChangesAsync() >= 0;
    }

  }
}