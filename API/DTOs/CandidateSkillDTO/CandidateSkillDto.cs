using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CandidateSkillDTO
{
    public class CandidateSkillDto
    {
        public int UniqueId { get; set; }
        
        public int CandidateId { get; set; }

        public string CandidateName { get; set; }

        public string SkillName { get; set; }

        public int SkillId { get; set; }

        public string Level { get; set; }
    }
}