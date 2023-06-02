using API.DTOs.SkillDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class StaffSkillController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public StaffSkillController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<StaffSkillDto>>> GetStaffSkills()
        {
            var staffSkills = await _context.StaffSkills
                .Include(s => s.Skill)
                .ToListAsync();

            var returnStaffSkills = _mapper.Map<List<StaffSkillDto>>(staffSkills);

            return returnStaffSkills;
        }

        [HttpGet("{id}", Name="GetStaffSkill")]
        public async Task<ActionResult<StaffSkillDto>> GetStaffSkill(int id)
        {
            var staffSkill = await _context.StaffSkills
                .Include(s => s.Skill)
                .FirstOrDefaultAsync(d => d.SkillId == id);
            
            var returnStaffSkill = _mapper.Map<StaffSkillDto>(staffSkill);

            return returnStaffSkill;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveStaffSkill(int id)
        {
            var staffSkill = await _context.StaffSkills
                .FirstOrDefaultAsync(d => d.UniqueId == id);
            
            if(staffSkill == null) return NotFound();

            _context.StaffSkills.Remove(staffSkill);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails {Title = "Problem removing Staff Skill"});
        }

        [HttpPost]
        public async Task<ActionResult> CreateStaffSkill([FromBody] StaffSkillDto staffSkill)
        {
            if(staffSkill == null) return BadRequest("Staff Skill data is missing");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var returnStaffSkill = _mapper.Map<StaffSkill>(staffSkill);

            _context.StaffSkills.Add(returnStaffSkill);
            
            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetStaffSkill), new {id = staffSkill.SkillId}, staffSkill);

            return BadRequest(new ProblemDetails {Title = "Problem adding staff skill"});
        }

        [HttpPatch]
        public async Task<ActionResult<StaffSkill>> UpdateSkill(int id, [FromBody] StaffSkillDto updatedStaffSkill)
        {
            if(updatedStaffSkill == null || updatedStaffSkill.UniqueId != id)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingStaffSkill = await _context.StaffSkills.FindAsync(id);

            if(existingStaffSkill == null) return NotFound("Department Not Found");

            existingStaffSkill.SkillId = updatedStaffSkill.SkillId;
            existingStaffSkill.Level = updatedStaffSkill.Level;

            _context.StaffSkills.Update(existingStaffSkill);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingStaffSkill);

            return BadRequest(new ProblemDetails {Title = "Problem Update Skill"});
        }
    }
}