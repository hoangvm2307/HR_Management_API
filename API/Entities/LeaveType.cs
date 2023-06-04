using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string? LeaveTypeName { get; set; }

    public string? LeaveTypeDetail { get; set; }

    public int? LeaveTypeMaxDay { get; set; }

    public virtual ICollection<LogLeave> LogLeaves { get; set; } = new List<LogLeave>();
}
