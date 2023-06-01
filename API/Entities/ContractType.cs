﻿using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class ContractType
{
    public int ContractTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PersonnelContract> PersonnelContracts { get; set; } = new List<PersonnelContract>();
}
