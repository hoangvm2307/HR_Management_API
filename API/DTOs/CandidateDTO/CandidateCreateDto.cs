using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CandidateDTO
{
    public class CandidateCreateDto
    {
        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? AppliedJob { get; set; }

        public string? AppliedDepartment { get; set; }

        public int? ExpectedSalary { get; set; }

        public string? ResumeFile { get; set; }
    }
}