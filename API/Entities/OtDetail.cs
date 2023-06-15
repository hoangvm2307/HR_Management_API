using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class OtDetail
{
    public int OtDetailId { get; set; }

    public int? PayslipId { get; set; }

    public int? OtTypeId { get; set; }

    public int? OtHours { get; set; }

    public int? OtAmount { get; set; }

    public virtual OtType? OtType { get; set; }
}
