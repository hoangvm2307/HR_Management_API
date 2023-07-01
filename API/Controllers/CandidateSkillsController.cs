using API.DTOs.CandidateSkillDTO;
using API.DTOs.SkillDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class CandidateSkillsController : BaseApiController
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    public CandidateSkillsController(SwpProjectContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CandidateSkillDto>>> GetCandidateSkills()
    {
      var candidateSkills = await _context.CandidateSkills
           .Include(c => c.Skill)
          .Select(s => new CandidateSkillDto
          {
            UniqueId = s.UniqueId,
            CandidateId = s.CandidateId,
            Level = s.Level,
            SkillId = s.SkillId,
            CandidateName = s.Candidate.Name,
            SkillName = s.Skill.SkillName
          })
          .ToListAsync();

      var candidateSkillDtos = _mapper.Map<List<CandidateSkillDto>>(candidateSkills);

      return candidateSkillDtos;
    }

    [HttpGet("candidate/{id}")]
    public async Task<ActionResult<List<CandidateSkillDto>>> GetCandidateSkillsByCandidateId(int id)
    {
      var candidateSkills = await _context.CandidateSkills
          .Include(c => c.Skill)
          .Where(c => c.CandidateId == id)
          .Select(s => new CandidateSkillDto
          {
            UniqueId = s.UniqueId,
            CandidateId = s.CandidateId,
            Level = s.Level,
            SkillId = s.SkillId,
            CandidateName = s.Candidate.Name,
            SkillName = s.Skill.SkillName
          })
          .ToListAsync();

      //var candidateSkillDtos = _mapper.Map<List<CandidateSkillDto>>(candidateSkills);

      return candidateSkills;
    }
    [HttpGet("{id}", Name = "GetCandidateSkillById")]
    public async Task<ActionResult<CandidateSkillDto>> GetCandidateSkillById(int id)
    {
      var candidateSkill = await _context.CandidateSkills
          .Include(c => c.Skill)
          .FirstOrDefaultAsync(d => d.UniqueId == id);

      if (candidateSkill == null) return NotFound();

      var returnStaffSkill = _mapper.Map<CandidateSkillDto>(candidateSkill);

      return returnStaffSkill;
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveCandidateSkill(int id)
    {
      var candidateSkill = await _context.CandidateSkills
          .FirstOrDefaultAsync(d => d.UniqueId == id);

      if (candidateSkill == null) return NotFound();

      _context.CandidateSkills.Remove(candidateSkill);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok();

      return BadRequest(new ProblemDetails { Title = "Problem removing Staff Skill" });
    }

    [HttpPost]
    public async Task<ActionResult> CreateCandidateSkill(CandidateSkillCreateDto candidateSkillDto)
    {
      if (candidateSkillDto == null) return BadRequest("Skill data is missing");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      // Create StaffSkills
      var existingSkill = await _context.Skills
              .FirstOrDefaultAsync(s => s.SkillName.ToLower().Trim()
                  .Equals(candidateSkillDto.SkillName.ToLower().Trim()));

      // If the skill does not exist, create a new one
      if (existingSkill == null)
      {
        existingSkill = new Skill
        {
          SkillName = candidateSkillDto.SkillName
        };

        _context.Skills.Add(existingSkill);
        await _context.SaveChangesAsync();
      }

      // Create the StaffSkill entry
      var candidateSkill = new CandidateSkill
      {
        CandidateId = candidateSkillDto.CandidateId,
        Level = candidateSkillDto.Level,
        SkillId = existingSkill.SkillId
      };

      // Save the StaffSkill in the database
      _context.CandidateSkills.Add(candidateSkill);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok("StaffSkills created successfully");

      return BadRequest();
    }

     [HttpPut]
    public async Task<IActionResult> UpdateCandidateSkillAndSkill(CandidateSkillUpdateDto candidateSkillDto)
    {
      // Retrieve the staff skill record to update based on the provided data
      var candidateSkill = await _context.CandidateSkills
          .FirstOrDefaultAsync(s => s.UniqueId == candidateSkillDto.UniqueId);

      if (candidateSkill == null) return NotFound("Candidate Skill Not Found");

      // Update the Level property
      if (!string.IsNullOrWhiteSpace(candidateSkillDto.Level))
        candidateSkill!.Level = candidateSkillDto.Level;

      // Retrieve the skill record to update based on the provided data
      if (!string.IsNullOrWhiteSpace(candidateSkillDto.SkillName))
      {
        var existingSkill = await _context.Skills
            .FirstOrDefaultAsync(s => s.SkillName.ToLower().Trim().Equals(candidateSkillDto.SkillName.ToLower().Trim()));

        // Update Skill ID Property
        if (existingSkill != null) candidateSkill!.SkillId = existingSkill.SkillId;
        else
        {
          // Create a new skill
          var newSkill = new Skill { SkillName = candidateSkillDto.SkillName };

          _context.Skills.Add(newSkill);

          await _context.SaveChangesAsync();

          // Update the SkillId property of the staff skill
          candidateSkill.SkillId = newSkill.SkillId;
        }
      }

      await _context.SaveChangesAsync();
      return Ok();
    }
  }
}