using API.DTOs.UserInforDTO;
using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PayslipDTOs
{
    public class PayslipDTO
    {
        public int PayslipId { get; set; }

        public int StaffId { get; set; }

        public int? BasicSalary { get; set; }

        public int? ActualSalary { get; set; }

        public double? StandardWorkDays { get; set; }

        public double? ActualWorkDays { get; set; }

        public double? LeaveHours { get; set; }

        public double? LeaveDays { get; set; }

        public int? OtTotal { get; set; }

        public int? GrossSalary { get; set; }

        public int? Bhxhemp { get; set; }

        public int? Bhytemp { get; set; }

        public int? Bhtnemp { get; set; }

        public int? SalaryBeforeTax { get; set; }

        public int? SelfAllowances { get; set; }

        public int? FamilyAllowances { get; set; }

        public int? SalaryTaxable { get; set; }

        public int? PersonalIncomeTax { get; set; }

        public int NetSalary { get; set; }

        public int? TotalAllowance { get; set; }

        public int? SalaryRecieved { get; set; }

        public int PaiByDate { get; set; }

        public int? Bhxhcomp { get; set; }

        public int? Bhytcomp { get; set; }

        public int? Bhtncomp { get; set; }

        public int? TotalInsured { get; set; }

        public int? TotalPaid { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? ChangeAt { get; set; }

        public bool? PayslipStatus { get; set; }

        public virtual UserInforDto Staff { get; set; } = null!;

        public virtual ICollection<TaxDetailDTO> TaxDetails { get; set; } = new List<TaxDetailDTO>();
    }
}