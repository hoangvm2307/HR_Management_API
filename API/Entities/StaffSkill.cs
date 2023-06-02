using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class StaffSkill
{
    public int UniqueId { get; set; }

    public int StaffId { get; set; }

    public int SkillId { get; set; }

    public string? Level { get; set; }

    public virtual Skill? Skill { get; set; } = null!;

    public virtual UserInfor Staff { get; set; } = null!;
}
