using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string? LeaveTypeName { get; set; }

    public string? LeaveTypeDetail { get; set; }

    public int? LaveTypeMaxDay { get; set; }

    public virtual ICollection<LeaveDayLeft> LeaveDayLefts { get; set; } = new List<LeaveDayLeft>();

    public virtual ICollection<LogLeave> LogLeaves { get; set; } = new List<LogLeave>();
}
