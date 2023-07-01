using API.DTOs.LogOtDTOs;
using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public PayslipExtensions(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }
        public PayslipExtensions()
        {

        }

        public async Task<PayslipDTO> ConvertGrossToNet(
                PersonnelContractDTO personnelContractDTO,
                int month,
                int year
            )
        {

            int taxableSalary = (int)personnelContractDTO.TaxableSalary;
            int StandardWorkDays = await GetStandardWorkDays(month, year);

            int PaiByDate = await GetPaifByDate(month, year, personnelContractDTO.Salary);
            int allowances = await GetTotalAllowancesByStaffId(personnelContractDTO.StaffId);
            double logOtHours = await GetLogOtHours(personnelContractDTO.StaffId);
            int LeaveDays = await GetLogLeaveDays(personnelContractDTO.StaffId);
            int logOtDays = await GetLogOtDays(personnelContractDTO.StaffId);


            int ActualWorkDays = StandardWorkDays + logOtDays - LeaveDays;

            //chỉnh sửa sau
            int noOfDependences = 0;
            
            var Insurance = PersonalInsuranceCalculate(taxableSalary);
            // var Insurance = PersonalInsuranceCalculate(LuongThoaThuanTrenHopDong);
            int grossSalary = personnelContractDTO.Salary;

            int ThuNhapTruocThue = ((int)(grossSalary - Insurance.SocialInsurance - Insurance.HealthInsurance - Insurance.UnemploymentInsurance));

            //Phu thuoc ca nhan, gia dinh, total
            int FamilyAllowancesDeduction = FamilyAllowances * noOfDependences;

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
            CompanyInsuranceDTO companyInsuranceDto = CompanyInsuranceCalculate(grossSalary, taxableSalary);

            PayslipDTO payslipDto = new PayslipDTO
            {
                // Contract = 2,
                GrossSalary = grossSalary,
                StandardWorkDays = StandardWorkDays,
                ActualWorkDays = ActualWorkDays,
                //OtHours = logOtHours,
                LeaveDays = LeaveDays,
                Bhxhemp = (int)Insurance.SocialInsurance,
                Bhytemp = (int)Insurance.HealthInsurance,
                Bhtnemp = (int)Insurance.UnemploymentInsurance,
                //can chinh sua self Allowances
                SelfAllowances = PersonalTaxDeduction,
                //NoOfDependences = noOfDependences,
                FamilyAllowances = FamilyAllowances,
                SalaryBeforeTax = ThuNhapTruocThue,
                //TaxRate5M = result.TaxRate5M,
                //TaxRate5Mto10M = result.TaxRate5MTo10M,
                //TaxRate10Mto18M = result.TaxRate10MTo18M,
                //TaxRate18Mto32M = result.TaxRate18MTo23M,
                //TaxRate32Mto52M = result.TaxRate23MTo52M,
                //TaxRate52Mto82M = result.TaxRate52MTo82M,
                //TaxRateOver82M = result.TaxRateOver82M,
                PersonalIncomeTax = ThueThuNhapCaNhan,
                //Bonus = allowances,
                // Deducion = 0,
                //TaxbleIncome = ThuNhapChiuThue,
                NetSalary = NetSalary,
                //tính lương 1 ngày
                PaiByDate = PaiByDate,
                CreateAt = CreatedDate,
                Bhxhcomp = (int)companyInsuranceDto.SocialInsurance,
                Bhytcomp = (int)companyInsuranceDto.HealthInsurance,
                Bhtncomp = (int)companyInsuranceDto.UnemploymentInsurance,
                TotalInsured = (int)companyInsuranceDto.Total,
                PayslipStatus = true
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
        private readonly ILogger<PayslipExtensions> logger;

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

        public CompanyInsuranceDTO CompanyInsuranceCalculate(int grossSalary, int taxableSalary)
        {
            int SocialInsuranceDeduction = (int)(taxableSalary * CompanySocialInsurance);
            int HealthInsuranceDeduction = (int)(taxableSalary * CompanyHealthInsurance);
            int UnemploymentInsuranceDeduction = (int)(taxableSalary * CompanyUnemploymentInsurance);

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

        public async Task<int> GetNumberWeekDaysInMonth(int month, int year)
        {
            var weekDays = await _context.DateDimensions.Where(c => c.IsWeekend == 0 && c.TheMonth == month && c.TheYear == year).CountAsync();

            return weekDays;
        }

        public async Task<int> GetNumberHolidaysInMonth(int month, int year)
        {
            var holidays = await _context.HolidayDimensions.Where(c => c.TheDate.Month == month && c.TheDate.Year == year).CountAsync();

            return holidays;
        }

        public async Task<int> GetStandardWorkDays(int month, int year)
        {
            var StandardWorkDays = await _context.TheCalendars
            .Where(c => c.IsWorking == 1 && c.TheMonth == month && c.TheYear == year).CountAsync();

            return StandardWorkDays;
        }

        public async Task<int> GetPaifByDate(int month, int year, int salary)
        {
            var StandardWorkDays = await _context.TheCalendars
           .Where(c => c.IsWorking == 1 && c.TheMonth == month && c.TheYear == year).CountAsync();

            int PaiByDate = 0;

            PaiByDate = salary / StandardWorkDays;

            return PaiByDate;
        }


        public async Task<double> GetLogOtHours(int StaffId)
        {
            var logOtHours = await _context.LogOts.Where(c => c.StaffId == StaffId && c.Status.ToLower().Equals("approved")).ToListAsync();

            double total = 0;
            total = logOtHours.Sum(c => c.LogHours);

            return total;
        }


        public async Task<int> GetLogOtDays(int StaffId)
        {
            var logOtStaff = await _context.LogOts.Where(c =>c.StaffId == StaffId && c.Status.ToLower().Equals("approved")).ToListAsync();

            int logOtDays = logOtStaff.Count();
            return logOtDays;
        }

        public async Task<int> GetLogLeaveHours(int StaffId)
        {
            var LeaveDays = await _context.LogLeaves.ToListAsync();

            var LogLeavesHours = LeaveDays.Sum(c => c.LeaveDays) * 8;

            return (int)LogLeavesHours;
        }

        public async Task<int> GetLogLeaveDays(int StaffId)
        {
            var LeaveDays = await _context.LogLeaves.ToListAsync();


            return (int)LeaveDays.Sum(c => c.LeaveDays);
        }

        public async Task<int> GetTotalAllowancesByStaffId(int StaffId)
        {
            var staffAllowances = await _context.PersonnelContracts.Include(c => c.Allowances).Where(c => c.StaffId == StaffId).ToListAsync();

            var allowances = staffAllowances.SelectMany(c => c.Allowances).Sum(c => c.AllowanceSalary);

            return (int)allowances;
        }
    }

}