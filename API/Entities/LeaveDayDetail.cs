using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class LeaveDayDetail
{
    public int LeaveDayDetailId { get; set; }

    public int? StaffId { get; set; }

    public int? LeaveTypeId { get; set; }

    public int? DayLeft { get; set; }

    public DateTime? ChangeAt { get; set; }

    public virtual LeaveType? LeaveType { get; set; }

    public virtual UserInfor? Staff { get; set; }
}
