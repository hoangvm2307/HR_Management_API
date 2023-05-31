using System;
using System.Collections.Generic;

namespace API.Models;

public partial class LogLeave
{
    public int LeaveLogId { get; set; }

    public int StaffId { get; set; }

    public DateTime LeaveStart { get; set; }

    public DateTime LeaveEnd { get; set; }

    public double LeaveDays { get; set; }

    public string? Reason { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserInfor Staff { get; set; } = null!;
}
