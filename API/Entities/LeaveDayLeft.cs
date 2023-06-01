using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class LeaveDayLeft
{
    public int LeaveDayLeftId { get; set; }

    public int? StaffId { get; set; }

    public int? LeaveTypeId { get; set; }

    public int? LeaveDayLeft1 { get; set; }

    public virtual LeaveType? LeaveType { get; set; }

    public virtual UserInfor? Staff { get; set; }
}
