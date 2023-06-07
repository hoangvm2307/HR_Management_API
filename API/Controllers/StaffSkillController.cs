using API.DTOs.SkillDTO;
using API.Entities;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<ActionResult> CreateStaffSkills([FromBody] SkillCreateDto skillCreateDto)
        {
            if (skillCreateDto == null) return BadRequest("Skill data is missing");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create StaffSkills
            if (skillCreateDto.StaffSkillCreateDtos != null)
            {
                foreach (var staffSkillDto in skillCreateDto.StaffSkillCreateDtos)
                {
                    // Check if the skill already exists
                    var existingSkill = await _context.Skills.FirstOrDefaultAsync(s => s.SkillName == skillCreateDto.SkillName);

                    // If the skill does not exist, create a new one
                    if (existingSkill == null)
                    {
                        existingSkill = new Skill
                        {
                            SkillName = skillCreateDto.SkillName
                        };

                        _context.Skills.Add(existingSkill);
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
                }

                await _context.SaveChangesAsync();
            }

            // Return a response indicating success
            return Ok("StaffSkills created successfully");
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
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateStaffSkillAndSkill(StaffSkillUpdateDto staffSkillUpdateDto)
        {
            // Retrieve the staff skill record to update based on the provided data
            var staffSkill = await _context.StaffSkills.FirstOrDefaultAsync(s => s.UniqueId == staffSkillUpdateDto.UniqueId);

            if (staffSkill != null)
            {
                // Update the Level property
                staffSkill.Level = staffSkillUpdateDto.Level;

                // Retrieve the skill record to update based on the provided data
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.SkillName.ToLower().Equals(staffSkillUpdateDto.SkillDto.SkillName));

                if (existingSkill != null)
                {
                    // Update the SkillName property
                    staffSkill.SkillId = existingSkill.SkillId;
                }
                else
                {
                    // Create a new skill
                    var newSkill = new Skill { SkillName = staffSkillUpdateDto.SkillDto?.SkillName };

                    _context.Skills.Add(newSkill);

                     await _context.SaveChangesAsync();
                     
                    // Update the SkillId property of the staff skill
                    staffSkill.SkillId = newSkill.SkillId;
                }

                var result = await _context.SaveChangesAsync() > 0;
                
                // Return successful save changes
                if(result) return Ok();

                return BadRequest("Problem updating");
            }
            else
            {
                // Return an error response if the staff skill record is not found
                return NotFound("Staff skill not found");
            }
        }

    }
}