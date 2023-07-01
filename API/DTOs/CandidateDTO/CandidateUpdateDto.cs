using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CandidateDTO
{
    public class CandidateUpdateDto
    {
        public string? Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? Dob { get; set; }

        public bool Gender { get; set; }

        public string Address { get; set; }

        public string Department { get; set; }

        public int? ExpectedSalary { get; set; }

        public string Result { get; set; }
    }
}