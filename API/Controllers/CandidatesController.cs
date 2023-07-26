using System.IO;
using API.DTOs.CandidateDTO;
using API.Entities;
using API.Extensions;
using API.RequestHelpers;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
  public class CandidatesController : BaseApiController
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    public CandidatesController(SwpProjectContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }
        [HttpGet]
        public async Task<ActionResult<PagedList<CandidateDto>>> GetCandidates(
        [FromQuery] CandidateParams candidateParams
        )
        {
            var candidates = _context.Candidates
            .Include(c => c.CandidateSkills)
            .OrderByDescending(c => c.CandidateId)
            .Search(candidateParams.SearchTerm)
            .Filter(candidateParams.Departments)
            .AsQueryable();

            var returnCandidates = await PagedList<Candidate>.ToPagedList(
                candidates,
                candidateParams.PageNumber,
                candidateParams.PageSize
                );

            var mappedCandidates = returnCandidates.Select(p => _mapper.Map<CandidateDto>(p)).ToList();


            //var candidateDtos = _mapper.Map<List<CandidateDto>>(candidates);

            mappedCandidates = mappedCandidates.Select(candidateDto =>
              {
                  candidateDto.DepartmentId = _context.Departments.Where
                    (d => d.DepartmentName.Trim().ToLower().Equals
                      (candidateDto.Department.Trim().ToLower())).
                      Select(d => d.DepartmentId).FirstOrDefault();

                  candidateDto.CandidateSkills = candidateDto.CandidateSkills.Select(candidateSkillDto =>
                  {
                      candidateSkillDto.SkillName = GetSkillNameByIdAsync(candidateSkillDto.SkillId).Result;
                      return candidateSkillDto;
                  }).ToList();
                  return candidateDto;
              }).ToList();

            var finalCandidates = new PagedList<CandidateDto>(
                mappedCandidates,
                returnCandidates.MetaData.TotalCount,
                candidateParams.PageNumber,
                candidateParams.PageSize);

            return finalCandidates;
        }

        [HttpGet("{id}", Name = "GetCandidateById")]
    public async Task<ActionResult<CandidateDto>> GetCandidateById(int id)
    {
      var candidate = await _context.Candidates
      .Include(c => c.CandidateSkills)
      .FirstOrDefaultAsync(c => c.CandidateId == id);

      if (candidate == null) return NotFound();

      var candidateDto = _mapper.Map<CandidateDto>(candidate);

      candidateDto.CandidateSkills = candidateDto.CandidateSkills.Select(candidateSkillDto =>
      {
        candidateSkillDto.SkillName = GetSkillNameByIdAsync(candidateSkillDto.SkillId).Result;
        return candidateSkillDto;
      }).ToList();

      return candidateDto;
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveCandidate(int id)
    {
      var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.CandidateId == id);

      if (candidate == null) return NotFound();

      candidate.Result = "Từ chối";

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return CreatedAtAction(nameof(GetCandidateById), new { id = candidate.CandidateId }, candidate);

      return BadRequest(new ProblemDetails { Title = "Problem removing candidate" });
    }

    [HttpPost]
    public async Task<ActionResult> CreateCandidate(CandidateCreateDto candidateDto)
    {
      if (candidateDto == null) return BadRequest("Candidate data is missing");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var candidate = new Candidate
      {
        Name = candidateDto.Name,
        Email = candidateDto.Email,
        Phone = candidateDto.Phone,
        Dob = candidateDto.Dob,
        Gender = candidateDto.Gender,
        Address = candidateDto.Address,
        Department = "",
        ExpectedSalary = candidateDto.ExpectedSalary,
        ResumeFile = candidateDto.ResumeFile,
        ApplyDate = DateTime.Now,
        Result = "Chờ duyệt"
      };

      _context.Candidates.Add(candidate);
      var result = await _context.SaveChangesAsync() > 0;

      if (result) return CreatedAtAction(nameof(GetCandidateById), new { id = candidate.CandidateId }, candidate);

      return BadRequest(new ProblemDetails { Title = "Problem adding candidate" });
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult> ApproveCandidate(int id, CandidateApproveDto candidateDto)
    {
      if (candidateDto == null) return BadRequest("Candidate Data is missing");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var candidate = await _context.Candidates.FindAsync(id);

      candidate.Department = candidateDto.Department;

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return CreatedAtAction(nameof(GetCandidateById), new { id = candidate.CandidateId }, candidate);

      return BadRequest(new ProblemDetails { Title = "Problem Approving Candidate" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCandidate(int id, CandidateUpdateDto candidateDto)
    {
      if (candidateDto == null) return BadRequest("Invalid Candidate Data");

      var candidate = await _context.Candidates.FindAsync(id);

      if (candidate == null) return NotFound("Candidate Not Found");

      candidate.Name = candidateDto.Name;
      candidate.Email = candidateDto.Email;
      candidate.Phone = candidateDto.Phone;
      candidate.Dob = candidateDto.Dob;
      candidate.Gender = candidateDto.Gender;
      candidate.Address = candidateDto.Address;
      candidate.Department = candidateDto.Department;
      candidate.ExpectedSalary = candidateDto.ExpectedSalary;
      candidate.Result = candidateDto.Result;


      var result = await _context.SaveChangesAsync() > 0;

      if (result) return CreatedAtAction(nameof(GetCandidateById), new { id = candidate.CandidateId }, candidate);

      return BadRequest(new ProblemDetails { Title = "Problem Updating Candidate" });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCandidate(int id, [FromBody] JsonPatchDocument<CandidateDto> patchDocument)
    {
      var candidate = await _context.Candidates.FindAsync(id);

      if (candidate == null) return NotFound();

      var candidateDto = _mapper.Map<CandidateDto>(candidate);

      patchDocument.ApplyTo(candidateDto, ModelState);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      _mapper.Map(candidateDto, candidate);

      var result = await _context.SaveChangesAsync() > 0;

      return NoContent();

    }
    private async Task<string> GetSkillNameByIdAsync(int id)
    {
      var skill = await _context.Skills.FirstOrDefaultAsync(x => x.SkillId == id);
      return skill?.SkillName;
    }

        [HttpGet("filters")]
        public async Task<IActionResult> Filter()
        {
            var departments = await _context.Candidates
                .Select(c => c.Department)
                .Distinct()
                .ToListAsync();
            return Ok(departments);
        }
    }
}