using API.DTOs.SkillDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class SkillController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public SkillController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SkillDto>>> GetSkills()
        {
            var skills = await _context.Skills.ToListAsync();

            var returnSkills = _mapper.Map<List<SkillDto>>(skills);

            return returnSkills;
        }

        [HttpGet("{id}", Name="GetSkill")]
        public async Task<ActionResult<SkillDto>> GetSkill(int id)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(d => d.SkillId == id);
            
            var returnSkill = _mapper.Map<SkillDto>(skill);

            return returnSkill;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveSkill(int id)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(d => d.SkillId == id);
            
            if(skill == null) return NotFound();

            _context.Skills.Remove(skill);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails {Title = "Problem removing skill"});
        }

        [HttpPost]
        public async Task<ActionResult> CreateSkill([FromBody] Skill skill)
        {
            if(skill == null) return BadRequest("Skill data is missing");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _context.Skills.Add(skill);
            
            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetSkill), new {id = skill.SkillId}, skill);

            return BadRequest(new ProblemDetails {Title = "Problem adding skill"});
        }

        [HttpPatch]
        public async Task<ActionResult<Skill>> UpdateSkill(int id, [FromBody] Skill updatedSkill)
        {
            if(updatedSkill == null || updatedSkill.SkillId != id)
            {
                return BadRequest("Invalid Skill Data");
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingSkill = await _context.Skills.FindAsync(id);

            if(existingSkill == null) return NotFound("Department Not Found");

            existingSkill.SkillName = updatedSkill.SkillName;

            _context.Skills.Update(existingSkill);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingSkill);

            return BadRequest(new ProblemDetails {Title = "Problem Update Skill"});
        }
    }
}