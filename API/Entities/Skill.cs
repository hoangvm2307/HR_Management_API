using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Skill
{
    public int SkillId { get; set; }

    public string? SkillName { get; set; }

    public string? SkillDescription { get; set; }

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    public virtual ICollection<StaffSkill> StaffSkills { get; set; } = new List<StaffSkill>();
}
