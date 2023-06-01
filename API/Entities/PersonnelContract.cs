using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class PersonnelContract
{
    public int ContractId { get; set; }

    public int StaffId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int Salary { get; set; }

    public int? WorkDatePerWeek { get; set; }

    public string? Note { get; set; }

    public int? ContractTypeId { get; set; }

    public int? SalaryTypeId { get; set; }

    public string? PaiDateNote { get; set; }

    public bool ContractStatus { get; set; }

    public virtual ICollection<Allowance> Allowances { get; set; } = new List<Allowance>();

    public virtual ContractType? ContractType { get; set; }

    public virtual ICollection<Payslip> Payslips { get; set; } = new List<Payslip>();

    public virtual SalaryType? SalaryType { get; set; }

    public virtual UserInfor Staff { get; set; } = null!;
}
