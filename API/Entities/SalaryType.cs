using System;
using System.Collections.Generic;

namespace API.Models;

public partial class SalaryType
{
    public int SalaryTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PersonnelContract> PersonnelContracts { get; set; } = new List<PersonnelContract>();
}
