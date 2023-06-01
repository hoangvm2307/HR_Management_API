using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class CandidateSkill
{
    public int UniqueId { get; set; }

    public int CandidateId { get; set; }

    public int SkillId { get; set; }

    public string? Level { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
