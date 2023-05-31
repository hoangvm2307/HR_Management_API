using System;
using System.Collections.Generic;

namespace API.Models;

public partial class LeaveDayLeft
{
    public int LeaveDayLeftId { get; set; }

    public int? StaffId { get; set; }

    public int? MaternityLeaveDayLeft { get; set; }

    public int? SickLeaveDayLeft { get; set; }

    public int? FuneralLeaveDayLeft { get; set; }

    public int? MarriageLeaveDayLeft { get; set; }

    public virtual UserInfor? Staff { get; set; }
}
