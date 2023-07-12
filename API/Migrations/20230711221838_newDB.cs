using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class newDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllowanceType",
                columns: table => new
                {
                    allowanceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allowanceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    allowanceDetailSalary = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowanceType_allowanceTypeId", x => x.allowanceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    candidateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imageFile = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    phone = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    dob = table.Column<DateTime>(type: "date", nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    address = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    department = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: false),
                    expectedSalary = table.Column<int>(type: "int", nullable: true),
                    resumeFile = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    applyDate = table.Column<DateTime>(type: "date", nullable: false),
                    result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate_candidateId", x => x.candidateId);
                });

            migrationBuilder.CreateTable(
                name: "ContractType",
                columns: table => new
                {
                    contractTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractType_contractTypeId", x => x.contractTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DateDimension",
                columns: table => new
                {
                    uniqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheDate = table.Column<DateTime>(type: "date", nullable: false),
                    TheDay = table.Column<int>(type: "int", nullable: true),
                    TheDayName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TheDayOfWeekInMonth = table.Column<byte>(type: "tinyint", nullable: true),
                    IsWeekend = table.Column<int>(type: "int", nullable: false),
                    TheWeek = table.Column<int>(type: "int", nullable: true),
                    TheDayOfWeek = table.Column<int>(type: "int", nullable: true),
                    TheMonth = table.Column<int>(type: "int", nullable: true),
                    TheMonthName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TheQuarter = table.Column<int>(type: "int", nullable: true),
                    TheYear = table.Column<int>(type: "int", nullable: true),
                    TheFirstOfYear = table.Column<DateTime>(type: "date", nullable: true),
                    TheFirstOfMonth = table.Column<DateTime>(type: "date", nullable: true),
                    Style101 = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DateDime__AA552EF373208D78", x => x.uniqueId)
                        .Annotation("SqlServer:Clustered", false);
                    table.UniqueConstraint("AK_DateDimension_TheDate", x => x.TheDate);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departmentName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department_departmentId", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                columns: table => new
                {
                    leaveTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    leaveTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    leaveTypeDetail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    leaveTypeDay = table.Column<int>(type: "int", nullable: true),
                    isSalary = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType_leaveTypeId", x => x.leaveTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OtType",
                columns: table => new
                {
                    otTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    typePercentage = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtType_otTypeId", x => x.otTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    roleId = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    roleName = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_roleId", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    skillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    skillName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill_skillId", x => x.skillId);
                });

            migrationBuilder.CreateTable(
                name: "TaxList",
                columns: table => new
                {
                    taxLevel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    taxRange = table.Column<int>(type: "int", nullable: true),
                    taxPercentage = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxList_taxLevel", x => x.taxLevel);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    ticketTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticketName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType_ticketTypeId", x => x.ticketTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HolidayDimension",
                columns: table => new
                {
                    TheDate = table.Column<DateTime>(type: "date", nullable: false),
                    HolidayText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_DateDimension",
                        column: x => x.TheDate,
                        principalTable: "DateDimension",
                        principalColumn: "TheDate");
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    roleId = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount_userId", x => x.userId);
                    table.ForeignKey(
                        name: "FK_UserAccount_roleId_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "roleId");
                });

            migrationBuilder.CreateTable(
                name: "CandidateSkill",
                columns: table => new
                {
                    uniqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    candidateId = table.Column<int>(type: "int", nullable: false),
                    skillId = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateSkill_uniqueId", x => x.uniqueId);
                    table.ForeignKey(
                        name: "FK_CandidateSkill_candidateId_candidateId",
                        column: x => x.candidateId,
                        principalTable: "Candidate",
                        principalColumn: "candidateId");
                    table.ForeignKey(
                        name: "FK_CandidateSkill_skillId_skillId",
                        column: x => x.skillId,
                        principalTable: "Skill",
                        principalColumn: "skillId");
                });

            migrationBuilder.CreateTable(
                name: "UserInfor",
                columns: table => new
                {
                    staffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    imageFile = table.Column<string>(type: "varchar(120)", unicode: false, maxLength: 120, nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    firstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    dob = table.Column<DateTime>(type: "date", nullable: true),
                    phone = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    address = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    country = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    citizenId = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    departmentId = table.Column<int>(type: "int", nullable: true),
                    hireDate = table.Column<DateTime>(type: "date", nullable: true),
                    bankAccount = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    bankAccountName = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: true),
                    bank = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    workTimeByYear = table.Column<int>(type: "int", nullable: true),
                    isManager = table.Column<bool>(type: "bit", nullable: true),
                    accountStatus = table.Column<bool>(type: "bit", nullable: true),
                    UserAccountUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfor_staffId", x => x.staffId);
                    table.ForeignKey(
                        name: "FK_UserInfor_UserAccount_UserAccountUserId",
                        column: x => x.UserAccountUserId,
                        principalTable: "UserAccount",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_UserInfor_departmentId_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Department",
                        principalColumn: "departmentId");
                    table.ForeignKey(
                        name: "FK_UserInfor_userId_userId",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveDayDetail",
                columns: table => new
                {
                    leaveDayDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: true),
                    leaveTypeId = table.Column<int>(type: "int", nullable: true),
                    dayLeft = table.Column<int>(type: "int", nullable: true),
                    changeAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    responseId = table.Column<int>(type: "int", nullable: true),
                    year = table.Column<int>(type: "int", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveDayDetail_leaveDayDetailId", x => x.leaveDayDetailId);
                    table.ForeignKey(
                        name: "FK_LeaveDayDetail_leaveTypeId_leaveTypeId",
                        column: x => x.leaveTypeId,
                        principalTable: "LeaveType",
                        principalColumn: "leaveTypeId");
                    table.ForeignKey(
                        name: "FK_LeaveDayDetail_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                });

            migrationBuilder.CreateTable(
                name: "LogLeave",
                columns: table => new
                {
                    leaveLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: false),
                    leaveTypeId = table.Column<int>(type: "int", nullable: true),
                    leaveStart = table.Column<DateTime>(type: "date", nullable: false),
                    leaveEnd = table.Column<DateTime>(type: "date", nullable: false),
                    leaveDays = table.Column<double>(type: "float", nullable: true),
                    leaveHours = table.Column<int>(type: "int", nullable: true),
                    salaryPerDay = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    processNote = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    respondencesId = table.Column<int>(type: "int", nullable: true),
                    changeStatusTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    enable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogLeave_leaveLogId", x => x.leaveLogId);
                    table.ForeignKey(
                        name: "FK_LogLeave_leaveTypeId_leaveTypeId",
                        column: x => x.leaveTypeId,
                        principalTable: "LeaveType",
                        principalColumn: "leaveTypeId");
                    table.ForeignKey(
                        name: "FK_LogLeave_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                });

            migrationBuilder.CreateTable(
                name: "LogOT",
                columns: table => new
                {
                    otLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: false),
                    otTypeId = table.Column<int>(type: "int", nullable: true),
                    logStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    logEnd = table.Column<DateTime>(type: "datetime", nullable: false),
                    logHours = table.Column<double>(type: "float", nullable: false),
                    days = table.Column<int>(type: "int", nullable: true),
                    salaryPerDay = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    processNote = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    respondencesId = table.Column<int>(type: "int", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    changeStatusTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    enable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogOT_otLogId", x => x.otLogId);
                    table.ForeignKey(
                        name: "FK_LogOT_otTypeId_otTypeId",
                        column: x => x.otTypeId,
                        principalTable: "OtType",
                        principalColumn: "otTypeId");
                    table.ForeignKey(
                        name: "FK_LogOT_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                });

            migrationBuilder.CreateTable(
                name: "Payslip",
                columns: table => new
                {
                    payslipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: false),
                    paidByDate = table.Column<int>(type: "int", nullable: false),
                    grossStandardSalary = table.Column<int>(type: "int", nullable: true),
                    grossActualSalary = table.Column<int>(type: "int", nullable: true),
                    standardWorkDays = table.Column<double>(type: "float", nullable: true),
                    actualWorkDays = table.Column<double>(type: "float", nullable: true),
                    leaveHours = table.Column<double>(type: "float", nullable: true),
                    leaveDays = table.Column<double>(type: "float", nullable: true),
                    otTotal = table.Column<int>(type: "int", nullable: true),
                    BHXHEmp = table.Column<int>(type: "int", nullable: true),
                    BHYTEmp = table.Column<int>(type: "int", nullable: true),
                    BHTNEmp = table.Column<int>(type: "int", nullable: true),
                    salaryBeforeTax = table.Column<int>(type: "int", nullable: true),
                    selfDeduction = table.Column<int>(type: "int", nullable: true),
                    familyDeduction = table.Column<int>(type: "int", nullable: true),
                    taxableSalary = table.Column<int>(type: "int", nullable: true),
                    personalIncomeTax = table.Column<int>(type: "int", nullable: true),
                    totalAllowance = table.Column<int>(type: "int", nullable: true),
                    salaryRecieved = table.Column<int>(type: "int", nullable: true),
                    netStandardSalary = table.Column<int>(type: "int", nullable: true),
                    netActualSalary = table.Column<int>(type: "int", nullable: true),
                    BHXHComp = table.Column<int>(type: "int", nullable: true),
                    BHYTComp = table.Column<int>(type: "int", nullable: true),
                    BHTNComp = table.Column<int>(type: "int", nullable: true),
                    totalCompInsured = table.Column<int>(type: "int", nullable: true),
                    totalCompPaid = table.Column<int>(type: "int", nullable: true),
                    createAt = table.Column<DateTime>(type: "date", nullable: true),
                    changeAt = table.Column<DateTime>(type: "date", nullable: true),
                    payslipStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payslip_payslipId", x => x.payslipId);
                    table.ForeignKey(
                        name: "FK_Payslip_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                });

            migrationBuilder.CreateTable(
                name: "PersonnelContract",
                columns: table => new
                {
                    contractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "date", nullable: false),
                    endDate = table.Column<DateTime>(type: "date", nullable: true),
                    taxableSalary = table.Column<int>(type: "int", nullable: true),
                    salary = table.Column<int>(type: "int", nullable: false),
                    workDatePerWeek = table.Column<int>(type: "int", nullable: true),
                    note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    noOfDependences = table.Column<int>(type: "int", nullable: true),
                    contractTypeId = table.Column<int>(type: "int", nullable: true),
                    salaryType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    contractFile = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    createAt = table.Column<DateTime>(type: "date", nullable: true),
                    responseId = table.Column<int>(type: "int", nullable: true),
                    changeAt = table.Column<DateTime>(type: "date", nullable: true),
                    contractStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelContract_contractId", x => x.contractId);
                    table.ForeignKey(
                        name: "FK_PersonnelContract_contractTypeId_contractTypeId",
                        column: x => x.contractTypeId,
                        principalTable: "ContractType",
                        principalColumn: "contractTypeId");
                    table.ForeignKey(
                        name: "FK_PersonnelContract_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                });

            migrationBuilder.CreateTable(
                name: "StaffSkill",
                columns: table => new
                {
                    uniqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: false),
                    skillId = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSkill_uniqueId", x => x.uniqueId);
                    table.ForeignKey(
                        name: "FK_StaffSkill_skillId_skillId",
                        column: x => x.skillId,
                        principalTable: "Skill",
                        principalColumn: "skillId");
                    table.ForeignKey(
                        name: "FK_StaffSkill_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ticketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staffId = table.Column<int>(type: "int", nullable: false),
                    ticketTypeId = table.Column<int>(type: "int", nullable: true),
                    ticketReason = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ticketFile = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    ticketStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    processNote = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    respondencesId = table.Column<int>(type: "int", nullable: true),
                    changeStatusTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    enable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket_ticketId", x => x.ticketId);
                    table.ForeignKey(
                        name: "FK_Ticket_staffId_staffId",
                        column: x => x.staffId,
                        principalTable: "UserInfor",
                        principalColumn: "staffId");
                    table.ForeignKey(
                        name: "FK_Ticket_ticketTypeId_ticketTypeId",
                        column: x => x.ticketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "ticketTypeId");
                });

            migrationBuilder.CreateTable(
                name: "TaxDetail",
                columns: table => new
                {
                    taxDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payslipId = table.Column<int>(type: "int", nullable: true),
                    taxLevel = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetail_taxDetailId", x => x.taxDetailId);
                    table.ForeignKey(
                        name: "FK_TaxDetail_payslipId_payslipId",
                        column: x => x.payslipId,
                        principalTable: "Payslip",
                        principalColumn: "payslipId");
                    table.ForeignKey(
                        name: "FK_TaxDetail_taxLevel_taxLevel",
                        column: x => x.taxLevel,
                        principalTable: "TaxList",
                        principalColumn: "taxLevel");
                });

            migrationBuilder.CreateTable(
                name: "Allowance",
                columns: table => new
                {
                    allowanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractId = table.Column<int>(type: "int", nullable: true),
                    allowanceTypeId = table.Column<int>(type: "int", nullable: true),
                    allowanceSalary = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allowance_allowanceId", x => x.allowanceId);
                    table.ForeignKey(
                        name: "FK_Allowance_allowanceTypeId_allowanceTypeId",
                        column: x => x.allowanceTypeId,
                        principalTable: "AllowanceType",
                        principalColumn: "allowanceTypeId");
                    table.ForeignKey(
                        name: "FK_Allowance_contractId_contractId",
                        column: x => x.contractId,
                        principalTable: "PersonnelContract",
                        principalColumn: "contractId");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f096456-655e-4868-ab18-20ed0172bada", null, "HRStaff", "HRSTAFF" },
                    { "8a909f36-bd23-40d6-ab91-fdb12a951ffd", null, "Staff", "STAFF" },
                    { "e9c76fbf-6097-41f7-b914-9724b4be220f", null, "HRManager", "HRMANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allowance_allowanceTypeId",
                table: "Allowance",
                column: "allowanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Allowance_contractId",
                table: "Allowance",
                column: "contractId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkill_candidateId",
                table: "CandidateSkill",
                column: "candidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkill_skillId",
                table: "CandidateSkill",
                column: "skillId");

            migrationBuilder.CreateIndex(
                name: "PK_DateDimension",
                table: "DateDimension",
                column: "TheDate",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "CIX_HolidayDimension",
                table: "HolidayDimension",
                column: "TheDate")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveDayDetail_leaveTypeId",
                table: "LeaveDayDetail",
                column: "leaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveDayDetail_staffId",
                table: "LeaveDayDetail",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_LogLeave_leaveTypeId",
                table: "LogLeave",
                column: "leaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LogLeave_staffId",
                table: "LogLeave",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_LogOT_otTypeId",
                table: "LogOT",
                column: "otTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LogOT_staffId",
                table: "LogOT",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslip_staffId",
                table: "Payslip",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelContract_contractTypeId",
                table: "PersonnelContract",
                column: "contractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelContract_staffId",
                table: "PersonnelContract",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSkill_skillId",
                table: "StaffSkill",
                column: "skillId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSkill_staffId",
                table: "StaffSkill",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetail_payslipId",
                table: "TaxDetail",
                column: "payslipId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetail_taxLevel",
                table: "TaxDetail",
                column: "taxLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_staffId",
                table: "Ticket",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ticketTypeId",
                table: "Ticket",
                column: "ticketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_roleId",
                table: "UserAccount",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserAccount_email",
                table: "UserAccount",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfor_departmentId",
                table: "UserInfor",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfor_Id",
                table: "UserInfor",
                column: "Id",
                unique: true,
                filter: "[Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfor_UserAccountUserId",
                table: "UserInfor",
                column: "UserAccountUserId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserInfor_citizenId",
                table: "UserInfor",
                column: "citizenId",
                unique: true,
                filter: "[citizenId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allowance");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CandidateSkill");

            migrationBuilder.DropTable(
                name: "HolidayDimension");

            migrationBuilder.DropTable(
                name: "LeaveDayDetail");

            migrationBuilder.DropTable(
                name: "LogLeave");

            migrationBuilder.DropTable(
                name: "LogOT");

            migrationBuilder.DropTable(
                name: "StaffSkill");

            migrationBuilder.DropTable(
                name: "TaxDetail");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AllowanceType");

            migrationBuilder.DropTable(
                name: "PersonnelContract");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "DateDimension");

            migrationBuilder.DropTable(
                name: "LeaveType");

            migrationBuilder.DropTable(
                name: "OtType");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Payslip");

            migrationBuilder.DropTable(
                name: "TaxList");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "ContractType");

            migrationBuilder.DropTable(
                name: "UserInfor");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
