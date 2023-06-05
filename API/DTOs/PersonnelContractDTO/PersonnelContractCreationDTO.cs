using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PersonnelContractDTO
{
    public class PersonnelContractCreationDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? TaxableSalary { get; set; }

        public int Salary { get; set; }

        public int? WorkDatePerWeek { get; set; }

        public string? Note { get; set; }

        public int? ContractTypeId { get; set; }

        public string? SalaryType { get; set; }

        public string? PaiDateNote { get; set; }

        public string? ContractFile { get; set; }

        public bool ContractStatus { get; set; }

        // public virtual ICollection<AllowancesDTO> Allowances { get; set; } = new List<AllowancesDTO>();

    }
}