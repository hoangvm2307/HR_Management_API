using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PersonnelContractDTO
{
    public class PersonnelContractDTO
    {
        public int ContractID { get; set; }
        public int StaffID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long Salary { get; set; }
        public long Allowances { get; set; }
        public int WorkDatePerMonth { get; set; }
        public string? Note { get; set; }
        public int ContractTypeID { get; set; }
        public int SalaryTypeID { get; set; }
        public string? ContractStatus { get; set; }
    }
}