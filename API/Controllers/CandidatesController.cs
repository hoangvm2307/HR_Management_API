using System.IO;
using API.DTOs.CandidateDTO;
using API.Entities;
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
    public async Task<ActionResult<List<CandidateDto>>> GetCandidates()
    {
      var candidates = await _context.Candidates.ToListAsync();

      if (candidates == null) return NotFound();

      var candidateDtos = _mapper.Map<List<CandidateDto>>(candidates);

      return candidateDtos;
    }

    [HttpGet("{id}", Name = "GetCandidateById")]
    public async Task<ActionResult<CandidateDto>> GetCandidateById(int id)
    {
      var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.CandidateId == id);

      if (candidate == null) return NotFound();

      var candidateDto = _mapper.Map<CandidateDto>(candidate);

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
    public async Task<IActionResult> PatchCandidate(int id, [FromBody] JsonPatchDocument<CandidateUpdateDto> patchDocument)
    {
      var candidate = await _context.Candidates.FindAsync(id);

      if (candidate == null) return NotFound();

      var candidateDto = _mapper.Map<CandidateUpdateDto>(candidate);

      patchDocument.ApplyTo(candidateDto, ModelState);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      _mapper.Map(candidateDto, candidate);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return NoContent();

      return BadRequest("Problem Updating Candidate");
    }
  }
}