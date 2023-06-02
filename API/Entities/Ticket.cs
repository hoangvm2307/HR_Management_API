﻿using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int StaffId { get; set; }

    public int? TicketTypeId { get; set; }

    public string TicketTitle { get; set; } = null!;

    public string? TicketFile { get; set; }

    public string TicketStatus { get; set; } = null!;

    public string? ProcessNote { get; set; }

    public int? RespondencesId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ChangeStatusTime { get; set; }

    public virtual UserInfor? Staff { get; set; } = null!;

    public virtual TicketType? TicketType { get; set; }
}
