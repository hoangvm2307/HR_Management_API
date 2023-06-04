using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class LeaveDayLeft
{
    public int LeaveDayLeftId { get; set; }

    public int? StaffId { get; set; }

    public int? FuneralLeaveDayLeftParents { get; set; }

    public int? FuneralLeaveDayLeftOthers { get; set; }

    public int? MarriageLeaveDayLeftSelf { get; set; }

    public int? MarriageLeaveDayLeftChild { get; set; }

    public int? MarriageLeaveDayLeftOthers { get; set; }

    public int? MaternityLeaveDayLeft { get; set; }

    public int? SickLeaveDayLeft { get; set; }

    public int? OtherLeaveDayLeft { get; set; }

    public int? YearLeaveDayLeft { get; set; }

    public virtual UserInfor? Staff { get; set; }
}
