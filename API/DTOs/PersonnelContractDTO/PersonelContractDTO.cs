using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs.PersonnelContractDTO
{
    public class PersonnelContractDTO
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

        public virtual ICollection<AllowancesDTO> Allowances { get; set; } = new List<AllowancesDTO>();
        public ContractTypeDTO? ContractType { get; set; }
        public virtual SalaryTypeDTO? SalaryType { get; set; }

    }
}