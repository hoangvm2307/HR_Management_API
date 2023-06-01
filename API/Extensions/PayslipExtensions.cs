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
        private static int PersonalTaxDeduction = 11000000;
        private static int FamilyAllowances = 4400000;
        private static double PersonalSocialInsurance = 0.08;
        private static double PersonalHealthInsurance = 0.015;
        private static double PersonalUnemploymentInsurance = 0.01;
        private static double CompanySocialInsurance = 0.175;
        private static double CompanyHealthInsurance = 0.03;
        private static double CompanyUnemploymentInsurance = 0.01;

        private static int PERSONAL_MAX_SOCIAL_INSURANCE_FEE = 2384000;
        private static int PERSONAL_MAX_HEALTH_INSURANCE_FEE = 447000;
        private static int PERSONAL_MAX_UNEMPLOYEMENT_INSURANCE_FEE = 884000;

        private static int COMPANY_MAX_SOCIAL_INSURANCE_FEE = 5215000;
        private static int COMPANY_MAX_HEALTH_INSURANCE_FEE = 894000;
        private static int COMPANY_MAX_UNEMPLOYEMENT_INSURANCE_FEE = 884000;


        public PayslipDTO ConvertGrossToNet(
            int grossSalary,
            int NoOfDependences = 0
            )
        {

            //Insurance Calculate
            int LuongThoaThuanTrenHopDong = 1490000;
            var Insurance = PersonalInsuranceCalculate(grossSalary);
            // var Insurance = PersonalInsuranceCalculate(LuongThoaThuanTrenHopDong);

            int ThuNhapTruocThue = ((int)(grossSalary - Insurance.SocialInsurance - Insurance.HealthInsurance - Insurance.UnemploymentInsurance));

            //Phu thuoc ca nhan, gia dinh, total
            int FamilyAllowancesDeduction = FamilyAllowances * NoOfDependences;

            int TotalAllowancesDeduction = (PersonalTaxDeduction + FamilyAllowancesDeduction);

            int ThuNhapChiuThue = ThuNhapTruocThue - TotalAllowancesDeduction;
            if (ThuNhapChiuThue < 0) ThuNhapChiuThue = 0;

            //Thue Thu Nhap Ca Nhan
            var result = PersonalIncomeTaxCalculate(ThuNhapChiuThue);

            int ThueThuNhapCaNhan = (int)result.Total();

            //Net Salary
            int NetSalary = ThuNhapTruocThue - ThueThuNhapCaNhan;

            DateTime CreatedDate = DateTime.UtcNow;

            //Nguoi su dung lao dong tra
            CompanyInsuranceDTO companyInsuranceDto = CompanyInsuranceCalculate(grossSalary);

            PayslipDTO payslipDto = new PayslipDTO
            {
                StaffId = 10000002,
                ContractId = 2,
                GrossSalary = grossSalary,
                StandardWorkDays = 0,
                ActualWorkDays = 0,
                OtHours = 0,
                LeaveDays = 0,
                BHYTEmp = (int)Insurance.HealthInsurance,
                BHXHEmp = (int)Insurance.HealthInsurance,
                BHTNEmp = (int)Insurance.UnemploymentInsurance,
                //can chinh sua self Allowances
                SelfAllowances = PersonalTaxDeduction,
                NoOfDependences = NoOfDependences,
                FamilyAllowances = FamilyAllowances,
                SalaryBeforeTax = ThuNhapTruocThue,
                TaxRate5M = result.TaxRate5M,
                TaxRate10MTo18M = result.TaxRate10MTo18M,
                TaxRate18MTo23M = result.TaxRate18MTo23M,
                TaxRate23MTo52M = result.TaxRate23MTo52M,
                TaxRate52MTo82M = result.TaxRate52MTo82M,
                TaxRateOver82M = result.TaxRateOver82M,
                Bonus = 0,
                Deducion = 0,
                NetSalary = NetSalary,
                PaidByDate = 0,
                PaidDate = CreatedDate,
                BHXHComp = (int)companyInsuranceDto.SocialInsurance,
                BHYTComp = (int)companyInsuranceDto.HealthInsurance,
                BHTNComp = (int)companyInsuranceDto.UnemploymentInsurance,
                TotalCompanyPaid = (int)companyInsuranceDto.Total,
                Status = true
            };

            return payslipDto;
        }


        (int, double)[] TaxableAmountAndTaxRate = {
                (5000000, 0.05),
                (5000000, 0.1),
                (8000000, 0.15),
                (14000000, 0.20),
                (20000000, 0.25),
                (28000000, 0.3),
                (0, 0.35),
            };

        public TaxRateDTO PersonalIncomeTaxCalculate(int ThuNhapChiuThue)
        {
            List<int> result = new List<int>();
            int TaxRate = 0;
            foreach (var number in TaxableAmountAndTaxRate)
            {
                if (number.Item1 == 0)
                {
                    TaxRate = (int)(ThuNhapChiuThue * number.Item2);
                }

                else if (ThuNhapChiuThue >= number.Item1)
                {
                    TaxRate = (int)(number.Item1 * number.Item2);
                }

                else
                {
                    TaxRate = (int)(ThuNhapChiuThue * number.Item2);
                }


                if (ThuNhapChiuThue <= 0) TaxRate = 0;

                result.Add(TaxRate);

                ThuNhapChiuThue -= number.Item1;
            }

            return new TaxRateDTO
            {
                TaxRate5M = result[0],
                TaxRate5MTo10M = result[1],
                TaxRate10MTo18M = result[2],
                TaxRate18MTo23M = result[3],
                TaxRate23MTo52M = result[4],
                TaxRate52MTo82M = result[5],
                TaxRateOver82M = result[6]
            };
        }


        //Customize Function
        public InsuranceDTO PersonalInsuranceCalculate(int InsuranceSalary)
        {
            int SocialInsuranceDeduction = (int)(InsuranceSalary * PersonalSocialInsurance);
            int HealthInsuranceDeduction = (int)(InsuranceSalary * PersonalHealthInsurance);
            int UnemploymentInsuranceDeduction = (int)(InsuranceSalary * PersonalUnemploymentInsurance);

            if (SocialInsuranceDeduction > PERSONAL_MAX_SOCIAL_INSURANCE_FEE)
            {
                SocialInsuranceDeduction = PERSONAL_MAX_SOCIAL_INSURANCE_FEE;
            }

            if (HealthInsuranceDeduction > PERSONAL_MAX_HEALTH_INSURANCE_FEE)
            {
                HealthInsuranceDeduction = PERSONAL_MAX_HEALTH_INSURANCE_FEE;
            }

            if (UnemploymentInsuranceDeduction > PERSONAL_MAX_UNEMPLOYEMENT_INSURANCE_FEE)
            {
                UnemploymentInsuranceDeduction = PERSONAL_MAX_UNEMPLOYEMENT_INSURANCE_FEE;
            }

            var InsuranceDeduction = new InsuranceDTO
            {
                SocialInsurance = SocialInsuranceDeduction,
                UnemploymentInsurance = UnemploymentInsuranceDeduction,
                HealthInsurance = HealthInsuranceDeduction,
            };

            return InsuranceDeduction;
        }

        public CompanyInsuranceDTO CompanyInsuranceCalculate(int grossSalary)
        {
            int SocialInsuranceDeduction = (int)(grossSalary * CompanySocialInsurance);
            int HealthInsuranceDeduction = (int)(grossSalary * CompanyHealthInsurance);
            int UnemploymentInsuranceDeduction = (int)(grossSalary * CompanyUnemploymentInsurance);

            if (SocialInsuranceDeduction > COMPANY_MAX_SOCIAL_INSURANCE_FEE)
            {
                SocialInsuranceDeduction = COMPANY_MAX_SOCIAL_INSURANCE_FEE;
            }

            if (HealthInsuranceDeduction > COMPANY_MAX_HEALTH_INSURANCE_FEE)
            {
                HealthInsuranceDeduction = COMPANY_MAX_HEALTH_INSURANCE_FEE;
            }

            if (UnemploymentInsuranceDeduction > COMPANY_MAX_UNEMPLOYEMENT_INSURANCE_FEE)
            {
                UnemploymentInsuranceDeduction = COMPANY_MAX_UNEMPLOYEMENT_INSURANCE_FEE;
            }

            int Total = grossSalary + (SocialInsuranceDeduction + HealthInsuranceDeduction + UnemploymentInsuranceDeduction);

            CompanyInsuranceDTO companyInsuranceDto = new CompanyInsuranceDTO
            {
                GrossSalary = grossSalary,
                SocialInsurance = SocialInsuranceDeduction,
                HealthInsurance = HealthInsuranceDeduction,
                UnemploymentInsurance = UnemploymentInsuranceDeduction,
                Total = Total
            };

            return companyInsuranceDto;
        }
    }

}