using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CandidateSkillDTO;

namespace API.DTOs.CandidateDTO
{
  public class CandidateDto
  {
    public int CandidateId { get; set; }

    public string? ImageFile { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? Dob { get; set; }

    public bool Gender { get; set; }

    public string GioiTinh => Gender ? "Nam" : "Nữ";

    public string? Address { get; set; }

    public string Department { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int? ExpectedSalary { get; set; }

    public string? ResumeFile { get; set; }

    public DateTime ApplyDate { get; set; }

    public string? Result { get; set; }
    public List<CandidateSkillDto> CandidateSkills { get; set; }
  }
}