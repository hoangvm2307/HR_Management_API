using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace API.Services
{
    public class PayslipService
    {
        private static int PersonalTaxDeduction = 11000000;
        private static int FamilyAllowances = 4400000;

        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly PersonnelContractService _personnelContractService;
        private readonly LogOtService _logOtService;
        private readonly LogLeaveService _logLeaveService;
        private readonly AllowanceService _allowanceService;

        public PayslipService(
            SwpProjectContext context, 
            IMapper mapper,
            ILogger<PayslipService> logger,
            PersonnelContractService personnelContractService,
            LogOtService logOtService,
            LogLeaveService logLeaveService,
            AllowanceService allowanceService
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
            _logOtService = logOtService ?? throw new ArgumentNullException(nameof(logOtService));
            _logLeaveService = logLeaveService ?? throw new ArgumentNullException(nameof(logLeaveService));
            _allowanceService = allowanceService ?? throw new ArgumentNullException(nameof(allowanceService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public async Task<PayslipDTO> AddPayslipToDatabase(int staffId, int month, int year)
        {
            var userInfo = await _context.UserInfors
                .Include(c => c.Payslips)
                .Where(c => c.StaffId == staffId && c.AccountStatus == true)
                .FirstOrDefaultAsync();

            if (userInfo == null) 
            {
                return null;
            }

            var payslipCreationDTO = await GetPayslipCreationDtoOfStaff(staffId, month, year);

            var payslipEntity = _mapper.Map<Payslip>(payslipCreationDTO);
            userInfo.Payslips.Add(payslipEntity);
            
            await _context.SaveChangesAsync();

            var returnPayslip = _mapper.Map<PayslipDTO>(payslipEntity);

            return returnPayslip;
        }
        public async Task<PayslipCreationDTO> GetPayslipCreationDtoOfStaff(
                int staffId,
                int month,
                int year
            )
        {

            var personnelContract = await _personnelContractService
                                        .GetValidPersonnelContractEntityByStaffId(staffId);
            int taxableSalaryFromContract = (int)personnelContract.TaxableSalary;

            var basicSalary = personnelContract.Salary;
            int standardWorkDays = await GetStandardWorkDays(month, year);
            int actualWorkDays = await GetActualWorkDaysOfStaff(staffId, month, year);

            int paidByDate = basicSalary / standardWorkDays;


            int leaveDays = await _logLeaveService.GetLeaveDays(staffId, month, year);
            int leaveHours = await _logLeaveService.GetLeavesHours(staffId, month, year);


            int grossSalary = await _personnelContractService.BasicGrossSalary(staffId);


            //lương nghỉ 

            int salaryDeduction = leaveDays * paidByDate;


            // tổng phúc lợi 
            int totalAllowance = await _allowanceService.GetAllowancesOfStaff(staffId);

            // tổng lương gross
            int totalGrossSalary =
                (grossSalary - salaryDeduction) +
                totalAllowance; 


            int totalTaxableSalary = taxableSalaryFromContract + totalAllowance;

            //Tính bảo hiểm từng phần
            var personalInsurance = PersonalInsuranceCalculate(totalTaxableSalary);

            //Thu nhập trước thuế (Tổng bảo hiểm cá nhân)
            int salaryBeforeTax = CalculatePretaxEarning(personalInsurance, totalGrossSalary);



            // Giảm trừ gia cảnh
            int selfAllowance = PersonalTaxDeduction;
            int familyAllowance = await _personnelContractService.GetFamilyAllowance(staffId);

            int taxableIncome = await TaxableIncomeCalculation(salaryBeforeTax, staffId);



            //Thue Thu Nhap Ca Nhan

            var result = PersonalIncomeTaxCalculate(taxableIncome);

            int personalIncomeTax = (int)result.Sum(c => c.Amount);


            //Net Salary
            int netSalary = (salaryBeforeTax - personalIncomeTax);


            // Lương thực nhận
            int otSalary = await _logOtService.OtSalary(staffId, month, year);
            int salaryRecevied = netSalary + otSalary;


            //Nguoi su dung lao dong tra
            CompanyInsuranceDTO companyInsuranceDto = CompanyInsuranceCalculate(totalGrossSalary, totalTaxableSalary);

            PayslipCreationDTO payslipDto = new PayslipCreationDTO
            {
                BasicSalary = basicSalary,
                ActualSalary = netSalary,
                StandardWorkDays = standardWorkDays,
                ActualWorkDays = actualWorkDays,
                LeaveHours = leaveHours,
                LeaveDays = leaveDays,
                OtTotal = otSalary,
                GrossSalary = totalGrossSalary,
                Bhxhemp = (int?)personalInsurance.SocialInsurance,
                Bhytemp = (int?)personalInsurance.HealthInsurance,
                Bhtnemp = (int?)personalInsurance.UnemploymentInsurance,
                SalaryBeforeTax = salaryBeforeTax,
                SelfAllowances = selfAllowance,
                FamilyAllowances = familyAllowance,
                SalaryTaxable = totalTaxableSalary,
                PersonalIncomeTax = personalIncomeTax,
                NetSalary = netSalary,
                TotalAllowance = totalAllowance,
                SalaryRecieved = salaryRecevied,
                PaiByDate = paidByDate,
                Bhxhcomp = (int?)companyInsuranceDto.SocialInsurance,
                Bhytcomp = (int?)companyInsuranceDto.HealthInsurance,
                Bhtncomp = (int?)companyInsuranceDto.UnemploymentInsurance,
                TotalInsured = (int?)companyInsuranceDto.Total,
                PayslipStatus = true
            };

            return payslipDto;
        }

        public async Task<int> TaxableIncomeCalculation(int PretaxEarning, int staffId)
        {

            int noOfDependences = await _personnelContractService
                                            .GetNoDependencies(staffId);

            int FamilyAllowancesDeduction = FamilyAllowances * noOfDependences;

            int TotalAllowancesDeduction = (PersonalTaxDeduction + FamilyAllowancesDeduction);

            int taxableIncome = PretaxEarning - TotalAllowancesDeduction;

            if (taxableIncome < 0) taxableIncome = 0;

            return taxableIncome;
        }

        public static int CalculatePretaxEarning(InsuranceDTO Insurance, int taxableSalary)
        {
            var totalInsurance = 
                Insurance.SocialInsurance + 
                Insurance.HealthInsurance + 
                Insurance.UnemploymentInsurance;
            return (int)(taxableSalary - totalInsurance);
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

        public List<TaxDetailCreationDTO> PersonalIncomeTaxCalculate(int ThuNhapChiuThue)
        {
            List<TaxDetailCreationDTO> result = new List<TaxDetailCreationDTO>();
            int TaxRate = 0;
            int i = 1;
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

                result.Add(new TaxDetailCreationDTO
                {
                    TaxLevel = i,
                    Amount = TaxRate
                });

                ThuNhapChiuThue -= number.Item1;
            }

            return result;
        }
        private static int PERSONAL_MAX_SOCIAL_INSURANCE_FEE = 2384000;
        private static int PERSONAL_MAX_HEALTH_INSURANCE_FEE = 447000;
        private static int PERSONAL_MAX_UNEMPLOYEMENT_INSURANCE_FEE = 884000;

        private static double PersonalSocialInsurance = 0.08;
        private static double PersonalHealthInsurance = 0.015;
        private static double PersonalUnemploymentInsurance = 0.01;
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


        private static int COMPANY_MAX_SOCIAL_INSURANCE_FEE = 5215000;
        private static int COMPANY_MAX_HEALTH_INSURANCE_FEE = 894000;
        private static int COMPANY_MAX_UNEMPLOYEMENT_INSURANCE_FEE = 884000;

        private static double CompanySocialInsurance = 0.175;
        private static double CompanyHealthInsurance = 0.03;
        private static double CompanyUnemploymentInsurance = 0.01;
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

            int Total = grossSalary + 
                (SocialInsuranceDeduction + HealthInsuranceDeduction + UnemploymentInsuranceDeduction);

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
            var weekDays = await _context.DateDimensions
                .Where(c => 
                    c.IsWeekend == 0 && 
                    c.TheMonth == month && 
                    c.TheYear == year)
                .CountAsync();

            return weekDays;
        }

        public async Task<int> GetNumberHolidaysInMonth(int month, int year)
        {
            var holidays = await _context.HolidayDimensions
                .Where(c => 
                c.TheDate.Month == month && 
                c.TheDate.Year == year)
                .CountAsync();

            return holidays;
        }


        public async Task<int> GetPaidByDate(int month, int year, int salary)
        {
            var StandardWorkDays = await _context.TheCalendars
               .Where(c => 
                    c.IsWeekend == 0 && 
                    c.TheMonth == month && 
                    c.TheYear == year)
               .CountAsync();

            int paidByDate = (salary / StandardWorkDays);

            return paidByDate;
        }


        public async Task<double> GetLogOtHours(int StaffId)
        {
            var logOtHours = await _context.LogOts
                .Where(c =>
                    c.StaffId == StaffId && 
                    c.Status.ToLower().Equals("approved"))
                .ToListAsync();

            double total = 0;
            total = logOtHours.Sum(c => c.LogHours);

            return total;
        }


        public async Task<int> GetLogOtDays(int StaffId)
        {
            var logOtStaff = await _context.LogOts
                .Where(c => 
                    c.StaffId == StaffId && 
                    c.Status.ToLower().Equals("approved"))
                .ToListAsync();

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
            var staffAllowances = await _context.PersonnelContracts
                .Include(c => c.Allowances).Where(c => c.StaffId == StaffId)
                .ToListAsync();

            var allowances = staffAllowances
                .SelectMany(c => c.Allowances)
                .Sum(c => c.AllowanceSalary);

            return (int)allowances;
        }

        public async Task<bool> IsGrossToNet(int staffId, int contractId)
        {
            return await _context.PersonnelContracts
                .AnyAsync(c => c.StaffId == staffId && c.ContractId == contractId && c.ContractStatus == true);
        }

        public async Task<List<PayslipDTO>> GetPayslipAsync()
        {
            var payslips = await _context.Payslips
                .Include(c => c.Staff)
                .Include(c => c.TaxDetails)
                .ToListAsync();

            var payslipsDTO = _mapper.Map<List<PayslipDTO>>(payslips);
            return payslipsDTO;
        }

        public async Task<List<PayslipDTO>> GetPayslipOfStaff(int staffId)
        {
            var payslips = await _context.Payslips
                .Where(c => c.StaffId == staffId)
                .ToListAsync();

            var finalPayslips = _mapper.Map<List<PayslipDTO>>(payslips);
            return finalPayslips;
        }

        public async Task<bool> IsPayslipExist(int staffId,int payslipId)
        {
            return await _context.Payslips
                .AnyAsync(c => c.StaffId == staffId && c.PayslipId == payslipId);
        }

        public async  Task<PayslipDTO> GetPayslipOfStaffByPayslipId(int staffId, int payslipId)
        {
            var payslip = await _context.Payslips
                .Where(c => c.StaffId == staffId && c.PayslipId == payslipId)
                .FirstOrDefaultAsync();

            var returnPayslip = _mapper.Map<PayslipDTO>(payslip);

            return returnPayslip;
        }

        //Số ngày đi làm cơ bản của 1 người trong 1 tháng
        public async Task<int> GetStandardWorkDays(int month, int year)
        {
            var StandardWorkDays = await _context.TheCalendars
                .Where(c =>
                    c.IsWorking == 1 &&
                    c.TheMonth == month &&
                    c.TheYear == year)
                .CountAsync();

            return StandardWorkDays;
        }

        //Số ngày đi làm trong tuần
        public async Task<int> GetStandardWorkDaysWithoutHoliday(int month, int year)
        {
            var basicActualWorkDays = await _context.TheCalendars
                .Where(c =>
                    c.TheMonth == month &&
                    c.TheYear == year &&
                    c.IsWeekend == 0)
                .ToListAsync();

            return basicActualWorkDays.Count;
        }

        public async Task<int> GetActualWorkDaysOfStaff(
            int staffId, int month, int year)
        {
            var basicActualWorkDays = await GetStandardWorkDays(month, year);

            var otDays = await _logOtService.GetOtDays(staffId, month, year);
            var leaveDays = await _logLeaveService.GetLeaveDays(staffId, month, year);

            int totalWorkingDays = basicActualWorkDays + otDays - leaveDays;

            if(totalWorkingDays < 0)
            {
                return 0;
            }
            else
            {
                return totalWorkingDays;
            }
        }

        
        

    }
}

