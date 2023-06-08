using API.DTOs.SkillDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
            var skills = await _context.Skills
                .Include(i => i.StaffSkills)
                .ToListAsync();

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
        public async Task<ActionResult> CreateSkill([FromBody] SkillCreateDto skillDto)
        {
            if(skillDto == null) return BadRequest("Skill data is missing");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var returnSkill = _mapper.Map<Skill>(skillDto);

            _context.Skills.Add(returnSkill);

            await _context.SaveChangesAsync(); // Save newly created skill to get skillId

            if (skillDto.StaffSkillCreateDtos != null)
            {
                foreach (var staffSkillDto in skillDto.StaffSkillCreateDtos)
                {
                    var staffSkill = new StaffSkill
                    {
                        StaffId = staffSkillDto.StaffId,
                        Level = staffSkillDto.Level,
                        SkillId = returnSkill.SkillId // Use the Skill ID from the newly created Skill
                    };

                    _context.StaffSkills.Add(staffSkill);
                }
            }
            
            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetSkill), new {id = returnSkill.SkillId}, returnSkill);

            return BadRequest(new ProblemDetails {Title = "Problem adding skill"});
        }

        [HttpPut]
        public async Task<ActionResult<Skill>> UpdateSkill(int id, [FromBody] SkillUpdateDto updatedSkill)
        {
            if(updatedSkill == null) return BadRequest("Invalid Skill Data");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingSkill = await _context.Skills.FindAsync(id);

            if(existingSkill == null) return NotFound("Department Not Found");

            existingSkill.SkillName = updatedSkill.SkillName;

            _context.Skills.Update(existingSkill);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingSkill);

            return BadRequest(new ProblemDetails {Title = "Problem Update Skill"});
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSkill(int id, [FromBody] JsonPatchDocument<SkillUpdateDto> patchDocument)
        {
            var skill = await _context.Skills.FindAsync(id);

            if (skill == null) return NotFound();

            var skillDto = _mapper.Map<SkillUpdateDto>(skill);

            patchDocument.ApplyTo(skillDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            _mapper.Map(skillDto, skill);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}