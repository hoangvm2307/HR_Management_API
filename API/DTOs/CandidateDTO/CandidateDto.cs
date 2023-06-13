using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CandidateDTO
{
    public class CandidateDto
    {
        public int CandidateId { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string Department { get; set; } = null!;

        public string? Company { get; set; }

        public int? ExpectedSalary { get; set; }

        public string? ResumeFile { get; set; }

        public DateTime ApplyDate { get; set; }

        public string? Result { get; set; }
    }
}