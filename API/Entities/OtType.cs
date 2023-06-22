using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class OtType
{
    public int OtTypeId { get; set; }

    public string? TypeName { get; set; }

    public double? TypePercentage { get; set; }

    public virtual ICollection<LogOt> LogOts { get; set; } = new List<LogOt>();
}
