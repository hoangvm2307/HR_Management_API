using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PersonnelContractDTO
{
    public class AllowanceTypeDTO
    {
        public int AllowanceTypeId { get; set; }

        public string? AllowanceName { get; set; }

        public string? AllowanceDetailSalary { get; set; }
    }
}