using System.Diagnostics;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(SwpProjectContext context, UserManager<User> userManager)
        {

            if (!context.Departments.Any())
            {

                var departments = new List<Department>()
                {
                  new Department{ DepartmentName = "Sales"},
                  new Department{ DepartmentName = "Marketing"},
                  new Department{ DepartmentName = "Finance"},
                  new Department{ DepartmentName = "Human Resource"},
                  new Department{ DepartmentName = "Operations"},
                  new Department{ DepartmentName = "Engineering"},
                  new Department{ DepartmentName = "Customer Support"},
                  new Department{ DepartmentName = "Research & Development"},
                  new Department{ DepartmentName = "Quality Assurance"},
                  new Department{ DepartmentName = "Design"},
                };

                foreach (var department in departments)
                {
                    await context.Departments.AddAsync(department);
                }
            }

            if (!context.UserInfors.Any())
            {
                var hrManager = new User
                {
                    UserName = "hrmanager",
                    Email = "hrmanager@test.com"
                };
                await userManager.CreateAsync(hrManager, "Pa$$w0rd");
                await userManager.AddToRolesAsync(hrManager, new[] { "Staff", "HRStaff", "HRManager" });

                var hrStaff = new User
                {
                    UserName = "hr",
                    Email = "hrstaff@test.com"
                };

                await userManager.CreateAsync(hrStaff, "Pa$$w0rd");
                await userManager.AddToRolesAsync(hrStaff, new[] { "Staff", "HRStaff" });

                var staff = new User
                {
                    UserName = "staff",
                    Email = "staff@test.com"
                };

                await userManager.CreateAsync(staff, "Pa$$w0rd");
                await userManager.AddToRoleAsync(staff, "Staff");

                var userInfors = new List<UserInfor>()
                {
                    new UserInfor{
                        Id = hrManager.Id,
                        LastName = "Nguyen Van",
                        FirstName = "Tong",
                        Dob = DateTime.Parse("2023-07-06T09:52:34.429Z"),
                        Phone = "string",
                        Gender = true,
                        Address = "string",
                        Country = "string",
                        CitizenId = "AAAA000001",
                        DepartmentId = 1,
                        IsManager = false,
                        HireDate =  DateTime.Parse("2023-07-06T09:52:34.429Z"),
                        BankAccount = "string",
                        BankAccountName = "string",
                        Bank = "string",
                        WorkTimeByYear = 360,
                        AccountStatus = true
                    },
                    new UserInfor{
                        Id =  hrStaff.Id,
                        LastName = "Nguyen Van",
                        FirstName = "Quan Ly",
                        Dob = DateTime.Parse("2003-07-06T09:52:34.429Z"),
                        Phone = "string",
                        Gender = true,
                        Address = "string",
                        Country = "string",
                        CitizenId = "AAAA000002",
                        DepartmentId = 1,
                        IsManager = true,
                        HireDate =  DateTime.Parse("2004-07-06T09:52:34.429Z"),
                        BankAccount = "string",
                        BankAccountName = "string",
                        Bank = "string",
                        WorkTimeByYear = 360,
                        AccountStatus = true
                    },
                    new UserInfor{
                        Id =  staff.Id,
                        LastName = "Nguyen Van",
                        FirstName = "Nhan Vien",
                        Dob = DateTime.Parse("2005-07-06T09:52:34.429Z"),
                        Phone = "string",
                        Gender = true,
                        Address = "string",
                        Country = "string",
                        CitizenId = "AAAA000003",
                        DepartmentId = 1,
                        IsManager = false,
                        HireDate =  DateTime.Parse("2023-07-06T09:52:34.429Z"),
                        BankAccount = "string",
                        BankAccountName = "string",
                        Bank = "string",
                        WorkTimeByYear = 360,
                        AccountStatus = true
                    },
                };

                foreach (var userInfor in userInfors)
                {
                    await context.UserInfors.AddAsync(userInfor);

                }
            }

            if (!context.TicketTypes.Any())
            {
                var ticketTypes = new List<TicketType>()
                {
                  new TicketType {TicketName = "Đơn xin tăng lương"},
                  new TicketType {TicketName = "Đơn xin cấp phát thiết bị làm việc"},
                  new TicketType {TicketName = "Đơn xin huấn luyện"},
                  new TicketType {TicketName = "Đơn xin điều chỉnh thời gian làm việc"},
                  new TicketType {TicketName = "Đơn xin xem xét lại quyền lợi"},
                  new TicketType {TicketName = "Đơn xin thăng chức"},
                  new TicketType {TicketName = "Đơn xin chuyển công tác"},
                };
                foreach (var ticketType in ticketTypes)
                {
                    await context.TicketTypes.AddAsync(ticketType);
                }
            }

            if (!context.ContractTypes.Any())
            {
                var contractTypes = new List<ContractType>()
                {
                    new ContractType{ Name = "Hợp đồng xác định hạn"},
                    new ContractType{ Name = "Hợp đồng không xác định hạn"}
                };

                foreach (var contract in contractTypes)
                {
                    await context.ContractTypes.AddAsync(contract);
                }
            }

            if (!context.AllowanceTypes.Any())
            {
                var allowanceTypes = new List<AllowanceType>()
                {
                    new AllowanceType{AllowanceName = "Phụ cấp trách nhiệm", AllowanceDetailSalary = "Không quá 10%"},
                    new AllowanceType{AllowanceName = "Phụ cấp thu hút", AllowanceDetailSalary = "Dưới 35%"},
                    new AllowanceType{AllowanceName = "Phụ cấp lưu động", AllowanceDetailSalary = "Dưới 10%"},
                    new AllowanceType{AllowanceName = "Phụ cấp chức vụ, chức danh", AllowanceDetailSalary = "Dưới 15%"},
                    new AllowanceType{AllowanceName = "Phụ cấp gửi xe và ăn trưa", AllowanceDetailSalary = "Tùy thỏa thuận"},
                };

                foreach (var allowanceType in allowanceTypes)
                {
                    await context.AllowanceTypes.AddAsync(allowanceType);
                }
            }

            if (!context.TaxLists.Any())
            {
                var taxLists = new List<TaxList>()
                {
                    new TaxList{Description = "Đến 5 triệu VND", TaxRange= 5000000,TaxPercentage=0.05},
                    new TaxList{Description = "Trên 5 triệu VND đến 10 triệu VND", TaxRange= 5000000,TaxPercentage=0.1},
                    new TaxList{Description = "Trên 10 triệu VND đến 18 triệu VND", TaxRange= 8000000,TaxPercentage=0.15},
                    new TaxList{Description = "Trên 18 triệu VND đến 32 triệu VND", TaxRange= 14000000,TaxPercentage=0.20},
                    new TaxList{Description = "Trên 32 triệu VND đến 52 triệu VND", TaxRange= 20000000,TaxPercentage=0.25},
                    new TaxList{Description = "Trên 52 triệu VND đến 80 triệu VND", TaxRange= 28000000,TaxPercentage=0.30},
                    new TaxList{Description = "Trên 80 triệu VND", TaxRange= 80000000,TaxPercentage=0.35},
                };

                foreach (var taxList in taxLists)
                {
                    await context.TaxLists.AddAsync(taxList);
                }
            }

            if (!context.OtTypes.Any())
            {
                var otTypes = new List<OtType>() {
                    new OtType{TypeName = "Làm thêm ngày cuối tuần", TypePercentage = 2},
                    new OtType{TypeName = "Làm thêm ngày lễ", TypePercentage = 3},
                    new OtType{TypeName = "Làm thêm ngày tết        ", TypePercentage = 4}
               };


                foreach (var otType in otTypes)
                {
                    await context.OtTypes.AddAsync(otType);
                }
            }

            if (!context.LeaveTypes.Any())
            {
                var leaveTypes = new List<LeaveType>() {

                new LeaveType{LeaveTypeName= "Nghỉ thai sản", LeaveTypeDetail="", LeaveTypeDay = 180,IsSalary = true},
                new LeaveType{LeaveTypeName= "Nghỉ phép có lương", LeaveTypeDetail="Nghỉ phép năm", LeaveTypeDay = 12,IsSalary = true},
                new LeaveType{LeaveTypeName= "Nghỉ phép không lương", LeaveTypeDetail="Nghỉ không lương", LeaveTypeDay = 20,IsSalary = false},
                };

                foreach (var leaveType in leaveTypes)
                {
                    await context.LeaveTypes.AddAsync(leaveType);
                }
            }

            if (!context.Candidates.Any())
            {
                var candidates = new List<Candidate>()
                {
                  new Candidate
                  {
                    Name = "John Cena",
                    Email = "john.cena@example.com",
                    Phone = "555-1234",
                  }
                };
            }




            await context.SaveChangesAsync();
        }
    }
}