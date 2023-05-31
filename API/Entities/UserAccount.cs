using System;
using System.Collections.Generic;

namespace API.Models;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual UserInfor? UserInfor { get; set; }
}
