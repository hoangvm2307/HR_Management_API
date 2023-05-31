using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PayslipDto :  InsuranceDto
    {
        //Lay trong hop dong
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public string SalaryType { get; set; } = string.Empty;
        public decimal FamilyAllowances { get; set; }
        public int NoOfDependences { get; set; }

        //Co dinh 8%, 1,5%, 1%
       

        //Mac dinh
        public decimal PersonalTaxDeduction { get; set; }
        //Tinh toan
        public decimal ThuNhapTruocThue { get; set; }
        public decimal ThuNhapChiuThue { get; set; }

        public decimal ThueThuNhapCaNhan { get; set; }

        //TaxRate
        public decimal TaxRate1 { get; set; }
        public decimal TaxRate2 { get; set; }
        public decimal TaxRate3 { get; set; }
        public decimal TaxRate4 { get; set; }
        public decimal TaxRate5 { get; set; }
        public decimal TaxRate6 { get; set; }
        public decimal TaxRate7 { get; set; }


    }
}