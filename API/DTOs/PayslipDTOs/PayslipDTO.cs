using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PayslipDTOs
{
    public class PayslipDTO
    {
        // public int PayslipId { get; set; }

        // public int StaffId { get; set; }

        public double? StandardWorkDays { get; set; }

        public double? ActualWorkDays { get; set; }

        public double? OtHours { get; set; }

        public double? LeaveDays { get; set; }

        public int? GrossSalary { get; set; }

        public int? Bhxhemp { get; set; }

        public int? Bhytemp { get; set; }

        public int? Bhtnemp { get; set; }

        public int? SalaryBeforeTax { get; set; }

        public int? NoOfDependences { get; set; }

        public int? SelfAllowances { get; set; }

        public int? FamilyAllowances { get; set; }

        public int? TaxbleIncome { get; set; }

        public int? TaxRate5M { get; set; }

        public int? TaxRate5Mto10M { get; set; }

        public int? TaxRate10Mto18M { get; set; }

        public int? TaxRate18Mto32M { get; set; }

        public int? TaxRate32Mto52M { get; set; }

        public int? TaxRate52Mto82M { get; set; }

        public int? TaxRateOver82M { get; set; }

        public int? PersonalIncomeTax { get; set; }

        public int? Bonus { get; set; }

        public int NetSalary { get; set; }

        public int PaiByDate { get; set; }

        public int? Bhxhcomp { get; set; }

        public int? Bhytcomp { get; set; }

        public int? Bhtncomp { get; set; }

        public int? TotalInsured { get; set; }

        public DateTime? CreateAt { get; set; }

        public bool? PayslipStatus { get; set; }

        // public virtual UserInfor Staff { get; set; } = null!;
    }
}