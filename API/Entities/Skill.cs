using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class Skill
{
    public int SkillId { get; set; }

    public string? SkillName { get; set; }

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();

    public virtual ICollection<StaffSkill> StaffSkills { get; set; } = new List<StaffSkill>();
}
