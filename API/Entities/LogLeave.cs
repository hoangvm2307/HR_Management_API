using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class LogLeave
{
    public int LeaveLogId { get; set; }

    public int StaffId { get; set; }

    public int? LeaveTypeId { get; set; }

    public DateTime LeaveStart { get; set; }

    public DateTime LeaveEnd { get; set; }

    public double LeaveDays { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual LeaveType? LeaveType { get; set; }

    public virtual UserInfor Staff { get; set; } = null!;
}
