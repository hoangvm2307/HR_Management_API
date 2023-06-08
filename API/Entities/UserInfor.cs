using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class UserInfor
{
    public int StaffId { get; set; }

    public string Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public string? Phone { get; set; }

    public bool Gender { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public string CitizenId { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public string? Position { get; set; }

    public DateTime HireDate { get; set; }

    public string BankAccount { get; set; } = null!;

    public string BankAccountName { get; set; } = null!;

    public string Bank { get; set; } = null!;

    public int? WorkTimeByYear { get; set; }

    public bool? AccountStatus { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<LeaveDayLeft> LeaveDayLefts { get; set; } = new List<LeaveDayLeft>();

    public virtual ICollection<LogLeave> LogLeaves { get; set; } = new List<LogLeave>();

    public virtual ICollection<LogOt> LogOts { get; set; } = new List<LogOt>();

    public virtual ICollection<Payslip> Payslips { get; set; } = new List<Payslip>();

    public virtual ICollection<PersonnelContract> PersonnelContracts { get; set; } = new List<PersonnelContract>();

    public virtual ICollection<StaffSkill> StaffSkills { get; set; } = new List<StaffSkill>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User User { get; set; } = null!;
}
