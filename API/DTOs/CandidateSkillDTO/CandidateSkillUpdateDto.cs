using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CandidateSkillDTO
{
    public class CandidateSkillUpdateDto
    {
        public int UniqueId { get; set;}
        
        public string? Level { get; set; }

        public string? SkillName { get; set; }
    }
}