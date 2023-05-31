﻿using System;
using System.Collections.Generic;

namespace API.Models;

public partial class LogOt
{
    public int OtLogId { get; set; }

    public int StaffId { get; set; }

    public string LogType { get; set; } = null!;

    public DateTime LogStart { get; set; }

    public DateTime LogEnd { get; set; }

    public double LogHours { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserInfor Staff { get; set; } = null!;
}