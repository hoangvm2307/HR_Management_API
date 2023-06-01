using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.DTOs.PayslipDTOs;

namespace API.Extensions
{
    public class PayslipExtensions
    {
        (decimal, decimal)[] TaxableAmountAndTaxRate = {
                (5000000, 0.05M),
                (5000000, 0.1M),
                (8000000, 0.15M),
                (14000000, 0.20M),
                (20000000, 0.25M),
                (28000000, 0.3M),
                (0, 0.35M),
            };


        private static decimal PersonalTaxDeduction = 11000000;
        private static decimal FamilyAllowances = 4400000;
        private static decimal SocialInsurance = 0.08M;
        private static decimal HealthInsurance = 0.015M;
        private static decimal UnemploymentInsurance = 0.01M;

        private static decimal MAX_SOCIAL_INSURANCE_FEE = 2384000;
        private static decimal MAX_HEALTH_INSURANCE_FEE = 447000;
        private static decimal MAX_UNEMPLOYEMENT_INSURANCE_FEE = 884000;

        public decimal TaxRate { get; set; }

        public TaxRateDTO PersonalIncomeTaxCalculate(decimal ThuNhapChiuThue)
        {
            List<decimal> result = new List<decimal>();
            decimal TaxRate = 0;
            foreach (var number in TaxableAmountAndTaxRate)
            {
                if (number.Item1 == 0)
                {
                    TaxRate = ThuNhapChiuThue * number.Item2;
                }

                else if (ThuNhapChiuThue >= number.Item1)
                {
                    TaxRate = number.Item1 * number.Item2;
                }

                else
                {
                    TaxRate = ThuNhapChiuThue * number.Item2;
                }


                if (ThuNhapChiuThue <= 0) TaxRate = 0;

                result.Add(TaxRate);

                ThuNhapChiuThue -= number.Item1;
            }

            return new TaxRateDTO
            {
                
            };
        }

        public PayslipDTO ConvertGrossToNet(
            decimal grossSalary,
            int NoOfDependences = 0
            )
        {
            decimal SocialInsuranceDeduction = grossSalary * SocialInsurance;
            decimal HealthInsuranceDeduction = grossSalary * HealthInsurance;
            decimal UnemploymentInsuranceDeduction = grossSalary * UnemploymentInsurance;

            if (SocialInsuranceDeduction > MAX_SOCIAL_INSURANCE_FEE)
            {
                SocialInsuranceDeduction = MAX_SOCIAL_INSURANCE_FEE;
            }

            if (HealthInsuranceDeduction > MAX_HEALTH_INSURANCE_FEE)
            {
                HealthInsuranceDeduction = MAX_HEALTH_INSURANCE_FEE;
            }

            if (UnemploymentInsuranceDeduction > MAX_UNEMPLOYEMENT_INSURANCE_FEE)
            {
                UnemploymentInsuranceDeduction = MAX_UNEMPLOYEMENT_INSURANCE_FEE;
            }

            decimal ThuNhapTruocThue = (grossSalary - SocialInsuranceDeduction - HealthInsuranceDeduction - UnemploymentInsuranceDeduction);

            decimal FamilyAllowancesDeduction = FamilyAllowances * NoOfDependences;

            decimal TotalAllowancesDeduction = PersonalTaxDeduction + FamilyAllowancesDeduction;


            decimal ThuNhapChiuThue = ThuNhapTruocThue - TotalAllowancesDeduction;
            if (ThuNhapChiuThue < 0) ThuNhapChiuThue = 0;

            //Thue Thu Nhap Ca Nhan
            var result = PersonalIncomeTaxCalculate(ThuNhapChiuThue);

            decimal ThueThuNhapCaNhan = result.Total();

            //Net Salary
            decimal NetSalary = ThuNhapTruocThue - ThueThuNhapCaNhan;

            return new PayslipDTO
            {
                // GrossSalary = grossSalary,
                // SocialInsurance = SocialInsuranceDeduction,
                // UnemploymentInsurance = UnemploymentInsuranceDeduction,
                // HealthInsurance = HealthInsuranceDeduction,
                // PersonalTaxDeduction = PersonalTaxDeduction,
                // FamilyAllowances = FamilyAllowancesDeduction,
                // NoOfDependences = NoOfDependences,
                // ThuNhapTruocThue = ThuNhapTruocThue,
                // ThuNhapChiuThue = ThuNhapChiuThue,
                // ThueThuNhapCaNhan = ThueThuNhapCaNhan,
                // NetSalary = NetSalary,
                // TaxRate1 = result.TaxRate1,
                // TaxRate2 = result.TaxRate2,
                // TaxRate3 = result.TaxRate3,
                // TaxRate4 = result.TaxRate4,
                // TaxRate5 = result.TaxRate5,
                // TaxRate6 = result.TaxRate6,
                // TaxRate7 = result.TaxRate7,
            };
        }
    }

}