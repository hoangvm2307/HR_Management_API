using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Entities;

public partial class Skill
{
    public int SkillId { get; set; }

    public string? SkillName { get; set; }

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    [JsonIgnore]
    public virtual ICollection<StaffSkill> StaffSkills { get; set; } = new List<StaffSkill>();
}
