using API.DTOs.UserInforDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class UserInforController : BaseApiController
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    public UserInforController(SwpProjectContext context, IMapper mapper, UserManager<User> userManager)
    {
      _mapper = mapper;
      _context = context;
      _userManager = userManager;
    }
    [HttpGet]
    public async Task<ActionResult<List<UserInforDto>>> GetUserInfors()
    {
      var userInfors = await _context.UserInfors
        .Include(c => c.StaffSkills)
        .ToListAsync();

      var userInforDtos = _mapper.Map<List<UserInforDto>>(userInfors);

      userInforDtos = userInforDtos.Select(userInforDto =>
      {
        userInforDto.Email = GetUserEmailByIdAsync(userInforDto.Id).Result;
        userInforDto.Position = userInforDto.IsManager ? "Manager" : "Staff";
        userInforDto.DepartmentName = GetDepartmentNameByIdAsync
                  (userInforDto.DepartmentId ?? 0).Result;
        userInforDto.PersonnelContract = GetPersonnelContractById(userInforDto.StaffId).Result;
        userInforDto.StaffSkills = userInforDto.StaffSkills.Select(staffSkillDto =>
        {
          staffSkillDto.SkillName = GetSkillNameByIdAsync(staffSkillDto.SkillId).Result;
          return staffSkillDto;
        }).ToList();

        return userInforDto;
      }).ToList();

      return userInforDtos;
    }

    [HttpGet("{id}", Name = "GetUserInforById")]
    public async Task<ActionResult<UserInforDto>> GetUserInforById(int id)
    {
      var userInfor = await _context.UserInfors
      .Include(c => c.StaffSkills)
          .FirstOrDefaultAsync(u => u.StaffId == id);

      if (userInfor == null) return NotFound();

      var userInforDto = _mapper.Map<UserInforDto>(userInfor);

      userInforDto.Email = GetUserEmailByIdAsync(userInforDto.Id).Result;
      userInforDto.Position = userInforDto.IsManager ? "Manager" : "Staff";
      userInforDto.PersonnelContract = GetPersonnelContractById(userInforDto.StaffId).Result;
      userInforDto.StaffSkills = userInforDto.StaffSkills.Select(staffSkillDto =>
        {
          staffSkillDto.SkillName = GetSkillNameByIdAsync(staffSkillDto.SkillId).Result;
          return staffSkillDto;
        }).ToList();
      return userInforDto;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveUserInfor(int id)
    {
      var staff = await _context.UserInfors
          .FirstOrDefaultAsync(u => u.StaffId == id);

      if (staff == null) return NotFound();

      staff.AccountStatus = false;

      _context.UserInfors.Update(staff);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok();

      return BadRequest(new ProblemDetails { Title = "Problem Removing User" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCandidate(int id, UserInforUpdateDto userInforDto)
    {
      if (userInforDto == null) return BadRequest("Invalid UserInfor Data");

      var userInfor = await _context.UserInfors.FindAsync(id);

      if (userInfor == null) return NotFound("Candidate UserInfor Found");

      userInfor.LastName = userInforDto.LastName;
      userInfor.FirstName = userInforDto.FirstName;
      // userInfor.DepartmentId = userInforDto.DeparatmentId; 
      userInfor.Dob = userInforDto.Dob;
      userInfor.Phone = userInforDto.Phone;
      userInfor.Gender = userInforDto.Gender;
      userInfor.Address = userInforDto.Address;
      userInfor.Country = userInforDto.Country;
      userInfor.CitizenId = userInforDto.CitizenId;
      userInfor.BankAccount = userInforDto.BankAccount;
      userInfor.BankAccountName = userInforDto.BankAccountName;
      userInfor.Bank = userInforDto.Bank;
      userInfor.AccountStatus = userInforDto.AccountStatus;

      var result = await _context.SaveChangesAsync() > 0;

      return CreatedAtAction(nameof(GetUserInforById), new { id = userInfor.StaffId }, userInfor);
    }
    // [HttpPost]
    // public async Task<ActionResult> CreateUserInfor([FromBody] UserInforDto userInforDto)
    // {
    //     var userInfor = new UserInfor
    //     {
    //         Id = user.Id,
    //         LastName = userInforDto.LastName,
    //         FirstName = userInforDto.FirstName,
    //         Dob = userInforDto.Dob,
    //         Gender = userInforDto.Gender,
    //         Address = userInforDto.Address,
    //         Country = userInforDto.Country,
    //         CitizenId = userInforDto.CitizenId,
    //         DepartmentId = userInforDto.DepartmentId,
    //         Position = userInforDto.Position,
    //         HireDate = userInforDto.HireDate,
    //         BankAccount = userInforDto.BankAccount,
    //         BankAccountName = userInforDto.BankAccountName,
    //         Bank = userInforDto.Bank,
    //         WorkTimeByYear = userInforDto.WorkTimeByYear,
    //         AccountStatus = userInforDto.AccountStatus
    //     };
    //     _context.UserInfors.Add(userInfor);
    //     if(await _context.SaveChangesAsync() > 0)
    //     {
    //         return StatusCode(201);
    //     }

    //     if(result) return CreatedAtAction(nameof(GetUserInforById), new {id = userInforDto.StaffId}, userInforDto);

    //     return BadRequest(new ProblemDetails {Title = "Problem adding user"});
    // }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUserInfor(int id, [FromBody] JsonPatchDocument<UserInforDto> patchDocument)
    {
      var userInfor = await _context.UserInfors.FindAsync(id);

      if (userInfor == null)
      {
        return NotFound();
      }

      var userInforDto = _mapper.Map<UserInforDto>(userInfor);

      patchDocument.ApplyTo(userInforDto, ModelState);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _mapper.Map(userInforDto, userInfor);

      await _context.SaveChangesAsync();

      return NoContent();
    }
    private async Task<string> GetUserEmailByIdAsync(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      return user?.Email;
    }
    private async Task<string> GetDepartmentNameByIdAsync(int id)
    {
      var department = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id);
      return department?.DepartmentName;
    }
    private async Task<string> GetSkillNameByIdAsync(int id)
    {
      var skill = await _context.Skills.FirstOrDefaultAsync(x => x.SkillId == id);
      return skill?.SkillName;
    }

    private async Task<PersonnelContract> GetPersonnelContractById(int id)
    {
      var personnelContract = await _context.PersonnelContracts.FirstOrDefaultAsync(x => x.StaffId == id);
      return personnelContract;
    }
  }
}