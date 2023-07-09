using API.DTOs.SkillDTO;
using API.DTOs.UserInforDTO;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class StaffSkillController : BaseApiController
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    private readonly UserService _userService;
    public StaffSkillController(SwpProjectContext context, IMapper mapper, UserService userService)
    {
      _userService = userService;
      _mapper = mapper;
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<StaffSkillDto>>> GetStaffSkills()
    {
      var staffSkills = await _context.StaffSkills
          .Include(s => s.Skill)
          .Include(s => s.Staff)
          .Select(s => new StaffSkillDto
          {
            UniqueId = s.UniqueId,
            StaffId = s.StaffId,
            DepartmentName = s.Staff.Department.DepartmentName,
            Level = s.Level,
            SkillId = s.SkillId,
            StaffName = s.Staff.LastName + " " + s.Staff.FirstName,
            SkillName = s.Skill.SkillName
          })
          .ToListAsync();

      var returnStaffSkills = _mapper.Map<List<StaffSkillDto>>(staffSkills);

      return staffSkills;
    }

    [HttpGet("{id}", Name = "GetStaffSkill")]
    public async Task<ActionResult<StaffSkillDto>> GetStaffSkill(int id)
    {
      var staffSkill = await _context.StaffSkills
          .Include(s => s.Skill)
          .FirstOrDefaultAsync(d => d.UniqueId == id);

      if (staffSkill == null) return NotFound();

      var returnStaffSkill = _mapper.Map<StaffSkillDto>(staffSkill);

      return returnStaffSkill;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveStaffSkill(int id)
    {
      var staffSkill = await _context.StaffSkills
          .FirstOrDefaultAsync(d => d.UniqueId == id);

      if (staffSkill == null) return NotFound();

      _context.StaffSkills.Remove(staffSkill);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok();

      return BadRequest(new ProblemDetails { Title = "Problem removing Staff Skill" });
    }

    [HttpGet("staffskill/{id}")]
    public async Task<ActionResult<List<StaffSkillDto>>> GetStaffSkillsByCandidateId(int id)
    {
      var staffSkills = await _context.StaffSkills
          .Include(c => c.Skill)
          .Where(c => c.StaffId == id)
          .Select(s => new StaffSkillDto
          {
            UniqueId = s.UniqueId,
            StaffId = s.StaffId,
            Level = s.Level,
            SkillId = s.SkillId,
            StaffName = s.Staff.LastName + " " + s.Staff.FirstName,
            SkillName = s.Skill.SkillName
          })
          .ToListAsync();

      //var candidateSkillDtos = _mapper.Map<List<CandidateSkillDto>>(candidateSkills);

      return staffSkills;
    }
    // [HttpPost]
    // public async Task<ActionResult> CreateStaffSkill(SkillCreateDto skillCreateDto)
    // {
    //     if (skillCreateDto == null) return BadRequest("Skill data is missing");

    //     if (!ModelState.IsValid) return BadRequest(ModelState);

    //     // Create StaffSkills
    //     if (skillCreateDto.StaffSkillCreateDtos != null)
    //     {
    //         foreach (var staffSkillDto in skillCreateDto.StaffSkillCreateDtos)
    //         {
    //             // Check if the skill already exists
    //             var existingSkill = await _context.Skills
    //                 .FirstOrDefaultAsync(s => s.SkillName.ToLower().Equals(skillCreateDto.SkillName.ToLower()));

    //             // If the skill does not exist, create a new one
    //             if (existingSkill == null)
    //             {
    //                 existingSkill = new Skill
    //                 {
    //                     SkillName = skillCreateDto.SkillName
    //                 };

    //                 _context.Skills.Add(existingSkill);
    //             }

    //             // Create the StaffSkill entry
    //             var staffSkill = new StaffSkill
    //             {
    //                 StaffId = staffSkillDto.StaffId,
    //                 Level = staffSkillDto.Level,
    //                 SkillId = existingSkill.SkillId
    //             };

    //             // Save the StaffSkill in the database
    //             _context.StaffSkills.Add(staffSkill);
    //         }

    //         await _context.SaveChangesAsync();
    //     }

    //     // Return a response indicating success
    //     return Ok("StaffSkills created successfully");
    // }
    [HttpPost]
    public async Task<ActionResult> CreateStaffSkill(StaffSkillCreateDto staffSkillDto)
    {
      if (staffSkillDto == null) return BadRequest("Skill data is missing");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      // Create StaffSkills
      // Check if the skill already exists
      var existingSkill = await _context.Skills
          .FirstOrDefaultAsync(s => s.SkillName.ToLower()
              .Equals(staffSkillDto.SkillName.ToLower()));


      // If the skill does not exist, create a new one
      if (existingSkill == null)
      {
        existingSkill = new Skill
        {
          SkillName = staffSkillDto.SkillName
        };

        _context.Skills.Add(existingSkill);

        await _context.SaveChangesAsync();
      }

      // Create the StaffSkill entry
      var staffSkill = new StaffSkill
      {
        StaffId = staffSkillDto.StaffId,
        Level = staffSkillDto.Level,
        SkillId = existingSkill.SkillId
      };

      // Save the StaffSkill in the database
      _context.StaffSkills.Add(staffSkill);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok("StaffSkills created successfully");

      return BadRequest();
    }





    // [HttpPatch("{id}")]
    // public async Task<ActionResult> UpdateStaffSkill(int id, [FromBody]JsonPatchDocument<StaffSkillUpdateDto> patchDocument)
    // {
    //     if(patchDocument == null) return BadRequest("Patch Document is missing");

    //     var staffSkill = await _context.StaffSkills.FindAsync(id);

    //     if(staffSkill == null) return NotFound();

    //     var staffSkillDto = _mapper.Map<StaffSkillUpdateDto>(staffSkill);

    //     patchDocument.ApplyTo(staffSkillDto, ModelState);

    //     if(!ModelState.IsValid) return BadRequest(ModelState); 

    //     _mapper.Map(staffSkillDto, staffSkill);

    //     var result = await _context.SaveChangesAsync() > 0;

    //     if(result) return Ok(staffSkill);

    //     return BadRequest(new ProblemDetails {Title = "Problem Update Staff Skill"});
    // }
    [HttpPut]
    public async Task<IActionResult> UpdateStaffSkillAndSkill(StaffSkillUpdateDto staffSkillUpdateDto)
    {
      // Retrieve the staff skill record to update based on the provided data
      var staffSkill = await _context.StaffSkills
          .FirstOrDefaultAsync(s => s.UniqueId == staffSkillUpdateDto.UniqueId);

      if (staffSkill == null) return NotFound("Staff Skill Not Found");

      // Update the Level property
      if (!string.IsNullOrWhiteSpace(staffSkillUpdateDto.Level))
        staffSkill.Level = staffSkillUpdateDto.Level;

      // Retrieve the skill record to update based on the provided data
      if (!string.IsNullOrWhiteSpace(staffSkillUpdateDto.SkillName))
      {
        var existingSkill = await _context.Skills
            .FirstOrDefaultAsync(s => s.SkillName.ToLower().Equals(staffSkillUpdateDto.SkillName.ToLower()));

        // Update Skill ID Property
        if (existingSkill != null) staffSkill.SkillId = existingSkill.SkillId;
        else
        {
          // Create a new skill
          var newSkill = new Skill { SkillName = staffSkillUpdateDto.SkillName };

          _context.Skills.Add(newSkill);

          await _context.SaveChangesAsync();

          // Update the SkillId property of the staff skill
          staffSkill.SkillId = newSkill.SkillId;
        }
      }

      await _context.SaveChangesAsync();
      return Ok();
    }

  }
}