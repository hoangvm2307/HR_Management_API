using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? AppliedJob { get; set; }

    public string? AppliedDepartment { get; set; }

    public string? AppliedCompany { get; set; } = "My Company";

    public string Department { get; set; } = null!;

    public string? Company { get; set; } = "My Company";

    public int? ExpectedSalary { get; set; }

    public int? ProposedSalary { get; set; }

    public string? ResumeFile { get; set; }

    public DateTime ApplyDate { get; set; }

    public string? Result { get; set; }

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
}
