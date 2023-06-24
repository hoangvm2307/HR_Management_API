using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CandidateSkillDTO
{
  public class CandidateSkillCreateDto
  {
    public int CandidateId { get; set; }
    public string SkillName { get; set; }
    public string? Level { get; set; }
  }
}