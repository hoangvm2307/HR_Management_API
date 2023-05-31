using System;
using System.Collections.Generic;

namespace API.Models;

public partial class AllowanceType
{
    public int AllowanceTypeId { get; set; }

    public string? AllowanceName { get; set; }

    public string? AllowanceDtailSalary { get; set; }

    public virtual ICollection<Allowance> Allowances { get; set; } = new List<Allowance>();
}
