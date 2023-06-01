using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Entities;

public partial class SwpProjectContext : DbContext
{
    public SwpProjectContext()
    {
    }

    public SwpProjectContext(DbContextOptions<SwpProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allowance> Allowances { get; set; }

    public virtual DbSet<AllowanceType> AllowanceTypes { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<ContractType> ContractTypes { get; set; }

    public virtual DbSet<DateDimension> DateDimensions { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<HolidayDimension> HolidayDimensions { get; set; }

    public virtual DbSet<LeaveDayLeft> LeaveDayLefts { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<LogLeave> LogLeaves { get; set; }

    public virtual DbSet<LogOt> LogOts { get; set; }

    public virtual DbSet<Payslip> Payslips { get; set; }

    public virtual DbSet<PersonnelContract> PersonnelContracts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalaryType> SalaryTypes { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<StaffSkill> StaffSkills { get; set; }

    public virtual DbSet<TheCalendar> TheCalendars { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserInfor> UserInfors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=swp_project;Uid=sa;Pwd=123456;Trusted_Connection=true;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allowance>(entity =>
        {
            entity.HasKey(e => e.AllowanceId).HasName("PK_Allowance_allowanceId");

            entity.ToTable("Allowance");

            entity.Property(e => e.AllowanceId).HasColumnName("allowanceId");
            entity.Property(e => e.AllowanceSalary).HasColumnName("allowanceSalary");
            entity.Property(e => e.AllowanceTypeId).HasColumnName("allowanceTypeId");
            entity.Property(e => e.ContractId).HasColumnName("contractId");

            entity.HasOne(d => d.AllowanceType).WithMany(p => p.Allowances)
                .HasForeignKey(d => d.AllowanceTypeId)
                .HasConstraintName("FK_Allowance_allowanceTypeId_allowanceTypeId");

            entity.HasOne(d => d.Contract).WithMany(p => p.Allowances)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK_Allowance_contractId_contractId");
        });

        modelBuilder.Entity<AllowanceType>(entity =>
        {
            entity.HasKey(e => e.AllowanceTypeId).HasName("PK_AllowanceType_allowanceTypeId");

            entity.ToTable("AllowanceType");

            entity.Property(e => e.AllowanceTypeId).HasColumnName("allowanceTypeId");
            entity.Property(e => e.AllowanceDetailSalary)
                .HasMaxLength(50)
                .HasColumnName("allowanceDetailSalary");
            entity.Property(e => e.AllowanceName)
                .HasMaxLength(50)
                .HasColumnName("allowanceName");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK_Candidate_candidateId");

            entity.ToTable("Candidate");

            entity.Property(e => e.CandidateId).HasColumnName("candidateId");
            entity.Property(e => e.AppliedCompany)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("appliedCompany");
            entity.Property(e => e.AppliedDepartment)
                .HasMaxLength(1)
                .HasColumnName("appliedDepartment");
            entity.Property(e => e.AppliedJob)
                .HasMaxLength(1)
                .HasColumnName("appliedJob");
            entity.Property(e => e.ApplyDate)
                .HasColumnType("date")
                .HasColumnName("applyDate");
            entity.Property(e => e.Company)
                .HasMaxLength(1)
                .HasColumnName("company");
            entity.Property(e => e.Department)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("email");
            entity.Property(e => e.ExpectedSalary).HasColumnName("expectedSalary");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.ProposedSalary).HasColumnName("proposedSalary");
            entity.Property(e => e.Result)
                .HasMaxLength(20)
                .HasColumnName("result");
            entity.Property(e => e.ResumeFile)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("resumeFile");
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.HasKey(e => e.UniqueId).HasName("PK_CandidateSkill_uniqueId");

            entity.ToTable("CandidateSkill");

            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.CandidateId).HasColumnName("candidateId");
            entity.Property(e => e.Level)
                .HasMaxLength(30)
                .HasColumnName("level");
            entity.Property(e => e.SkillId).HasColumnName("skillId");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CandidateSkill_candidateId_candidateId");

            entity.HasOne(d => d.Skill).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CandidateSkill_skillId_skillId");
        });

        modelBuilder.Entity<ContractType>(entity =>
        {
            entity.HasKey(e => e.ContractTypeId).HasName("PK_ContractType_contractTypeId");

            entity.ToTable("ContractType");

            entity.Property(e => e.ContractTypeId).HasColumnName("contractTypeId");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DateDimension>(entity =>
        {
            entity.HasKey(e => e.UniqueId)
                .HasName("PK__DateDime__AA552EF3A11801FE")
                .IsClustered(false);

            entity.ToTable("DateDimension");

            entity.HasIndex(e => e.TheDate, "PK_DateDimension")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.Style101)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TheDate)
                .IsRequired()
                .HasColumnType("date");
            entity.Property(e => e.TheDayName).HasMaxLength(30);
            entity.Property(e => e.TheFirstOfMonth).HasColumnType("date");
            entity.Property(e => e.TheFirstOfYear).HasColumnType("date");
            entity.Property(e => e.TheMonthName).HasMaxLength(30);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK_Department_departmentId");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasColumnName("departmentId");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(25)
                .HasColumnName("departmentName");
        });

        modelBuilder.Entity<HolidayDimension>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HolidayDimension");

            entity.HasIndex(e => e.TheDate, "CIX_HolidayDimension").IsClustered();

            entity.Property(e => e.HolidayText).HasMaxLength(255);
            entity.Property(e => e.TheDate).HasColumnType("date");

            entity.HasOne(d => d.TheDateNavigation).WithMany()
                .HasPrincipalKey(p => p.TheDate)
                .HasForeignKey(d => d.TheDate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DateDimension");
        });

        modelBuilder.Entity<LeaveDayLeft>(entity =>
        {
            entity.HasKey(e => e.LeaveDayLeftId).HasName("PK_LeaveDayLeft_leaveDayLeftId");

            entity.ToTable("LeaveDayLeft");

            entity.Property(e => e.LeaveDayLeftId).HasColumnName("leaveDayLeftId");
            entity.Property(e => e.LeaveDayLeft1).HasColumnName("leaveDayLeft");
            entity.Property(e => e.LeaveTypeId).HasColumnName("leaveTypeId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveDayLefts)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_LeaveDayLeft_leaveTypeId_leaveTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.LeaveDayLefts)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_LeaveDayLeft_staffId_staffId");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId).HasName("PK_LeaveType_leaveTypeId");

            entity.ToTable("LeaveType");

            entity.Property(e => e.LeaveTypeId).HasColumnName("leaveTypeId");
            entity.Property(e => e.LaveTypeMaxDay).HasColumnName("laveTypeMaxDay");
            entity.Property(e => e.LeaveTypeDetail)
                .HasMaxLength(130)
                .HasColumnName("leaveTypeDetail");
            entity.Property(e => e.LeaveTypeName)
                .HasMaxLength(30)
                .HasColumnName("leaveTypeName");
        });

        modelBuilder.Entity<LogLeave>(entity =>
        {
            entity.HasKey(e => e.LeaveLogId).HasName("PK_LogLeave_leaveLogId");

            entity.ToTable("LogLeave");

            entity.Property(e => e.LeaveLogId).HasColumnName("leaveLogId");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.Description)
                .HasMaxLength(70)
                .HasColumnName("description");
            entity.Property(e => e.LeaveDays).HasColumnName("leaveDays");
            entity.Property(e => e.LeaveEnd)
                .HasColumnType("date")
                .HasColumnName("leaveEnd");
            entity.Property(e => e.LeaveStart)
                .HasColumnType("date")
                .HasColumnName("leaveStart");
            entity.Property(e => e.LeaveTypeId).HasColumnName("leaveTypeId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LogLeaves)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_LogLeave_leaveTypeId_leaveTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.LogLeaves)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogLeave_staffId_staffId");
        });

        modelBuilder.Entity<LogOt>(entity =>
        {
            entity.HasKey(e => e.OtLogId).HasName("PK_LogOT_otLogId");

            entity.ToTable("LogOT");

            entity.Property(e => e.OtLogId).HasColumnName("otLogId");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Description)
                .HasMaxLength(25)
                .HasColumnName("description");
            entity.Property(e => e.LogEnd)
                .HasColumnType("datetime")
                .HasColumnName("logEnd");
            entity.Property(e => e.LogHours).HasColumnName("logHours");
            entity.Property(e => e.LogStart)
                .HasColumnType("datetime")
                .HasColumnName("logStart");
            entity.Property(e => e.LogTitile)
                .HasMaxLength(20)
                .HasColumnName("logTitile");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.Staff).WithMany(p => p.LogOts)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogOT_staffId_staffId");
        });

        modelBuilder.Entity<Payslip>(entity =>
        {
            entity.HasKey(e => e.PayslipId).HasName("PK_Payslip_payslipId");

            entity.ToTable("Payslip");

            entity.Property(e => e.PayslipId).HasColumnName("payslipId");
            entity.Property(e => e.ActualWorkDays).HasColumnName("actualWorkDays");
            entity.Property(e => e.Bhtncomp).HasColumnName("BHTNComp");
            entity.Property(e => e.Bhtnemp).HasColumnName("BHTNEmp");
            entity.Property(e => e.Bhxhcomp).HasColumnName("BHXHComp");
            entity.Property(e => e.Bhxhemp).HasColumnName("BHXHEmp");
            entity.Property(e => e.Bhytcomp).HasColumnName("BHYTComp");
            entity.Property(e => e.Bhytemp).HasColumnName("BHYTEmp");
            entity.Property(e => e.Bonus).HasColumnName("bonus");
            entity.Property(e => e.ContractId).HasColumnName("contractId");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.Deduction).HasColumnName("deduction");
            entity.Property(e => e.FamilyAllowances).HasColumnName("familyAllowances");
            entity.Property(e => e.GrossSalary).HasColumnName("grossSalary");
            entity.Property(e => e.LeaveDays).HasColumnName("leaveDays");
            entity.Property(e => e.NetSalary).HasColumnName("netSalary");
            entity.Property(e => e.NoOfDependences).HasColumnName("noOfDependences");
            entity.Property(e => e.OtHours).HasColumnName("otHours");
            entity.Property(e => e.PaiByDate).HasColumnName("paiByDate");
            entity.Property(e => e.PaidDate)
                .HasColumnType("date")
                .HasColumnName("paidDate");
            entity.Property(e => e.PayslipStatus).HasColumnName("payslipStatus");
            entity.Property(e => e.SalaryBeforeTax).HasColumnName("salaryBeforeTax");
            entity.Property(e => e.SelfAllowances).HasColumnName("selfAllowances");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.StandardWorkDays).HasColumnName("standardWorkDays");
            entity.Property(e => e.TaxRate10Mto18M).HasColumnName("taxRate10MTo18M");
            entity.Property(e => e.TaxRate18Mto32M).HasColumnName("taxRate18MTo32M");
            entity.Property(e => e.TaxRate32Mto52M).HasColumnName("taxRate32MTo52M");
            entity.Property(e => e.TaxRate52Mto82M).HasColumnName("taxRate52MTo82M");
            entity.Property(e => e.TaxRate5M).HasColumnName("taxRate5M");
            entity.Property(e => e.TaxRate5Mto10M).HasColumnName("taxRate5MTo10M");
            entity.Property(e => e.TaxRateOver82M).HasColumnName("taxRateOver82M");

            entity.HasOne(d => d.Contract).WithMany(p => p.Payslips)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payslip_contractId_contractId");

            entity.HasOne(d => d.Staff).WithMany(p => p.Payslips)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payslip_staffId_staffId");
        });

        modelBuilder.Entity<PersonnelContract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK_PersonnelContract_contractId");

            entity.ToTable("PersonnelContract");

            entity.Property(e => e.ContractId).HasColumnName("contractId");
            entity.Property(e => e.ContractStatus).HasColumnName("contractStatus");
            entity.Property(e => e.ContractTypeId).HasColumnName("contractTypeId");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.Note)
                .HasMaxLength(1)
                .HasColumnName("note");
            entity.Property(e => e.PaiDateNote)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("paiDateNote");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.SalaryTypeId).HasColumnName("salaryTypeId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
            entity.Property(e => e.WorkDatePerWeek).HasColumnName("workDatePerWeek");

            entity.HasOne(d => d.ContractType).WithMany(p => p.PersonnelContracts)
                .HasForeignKey(d => d.ContractTypeId)
                .HasConstraintName("FK_PersonnelContract_contractTypeId_contractTypeId");

            entity.HasOne(d => d.SalaryType).WithMany(p => p.PersonnelContracts)
                .HasForeignKey(d => d.SalaryTypeId)
                .HasConstraintName("FK_PersonnelContract_salaryTypeId_salaryTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.PersonnelContracts)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonnelContract_staffId_staffId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Role_roleId");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<SalaryType>(entity =>
        {
            entity.HasKey(e => e.SalaryTypeId).HasName("PK_SalaryType_salaryTypeId");

            entity.ToTable("SalaryType");

            entity.Property(e => e.SalaryTypeId).HasColumnName("salaryTypeId");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK_Skill_skillId");

            entity.ToTable("Skill");

            entity.Property(e => e.SkillId).HasColumnName("skillId");
            entity.Property(e => e.SkillName)
                .HasMaxLength(20)
                .HasColumnName("skillName");
        });

        modelBuilder.Entity<StaffSkill>(entity =>
        {
            entity.HasKey(e => e.UniqueId).HasName("PK_StaffSkill_uniqueId");

            entity.ToTable("StaffSkill");

            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.Level)
                .HasMaxLength(45)
                .HasColumnName("level");
            entity.Property(e => e.SkillId).HasColumnName("skillId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");

            entity.HasOne(d => d.Skill).WithMany(p => p.StaffSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffSkill_skillId_skillId");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffSkills)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffSkill_staffId_staffId");
        });

        modelBuilder.Entity<TheCalendar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TheCalendar");

            entity.Property(e => e.HolidayText).HasMaxLength(255);
            entity.Property(e => e.IsWorking).HasColumnName("isWorking");
            entity.Property(e => e.Style101)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TheDate).HasColumnType("date");
            entity.Property(e => e.TheDayName).HasMaxLength(30);
            entity.Property(e => e.TheFirstOfMonth).HasColumnType("date");
            entity.Property(e => e.TheFirstOfYear).HasColumnType("date");
            entity.Property(e => e.TheMonthName).HasMaxLength(30);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK_Ticket_ticketId");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("ticketId");
            entity.Property(e => e.ChangeStatusTime)
                .HasColumnType("datetime")
                .HasColumnName("changeStatusTime");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.ProcessNote)
                .HasMaxLength(1)
                .HasColumnName("processNote");
            entity.Property(e => e.RespondencesId).HasColumnName("respondencesId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.TicketFile)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ticketFile");
            entity.Property(e => e.TicketStatus)
                .HasMaxLength(20)
                .HasColumnName("ticketStatus");
            entity.Property(e => e.TicketTitle)
                .HasMaxLength(40)
                .HasColumnName("ticketTitle");
            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_staffId_staffId");

            entity.HasOne(d => d.TicketType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketTypeId)
                .HasConstraintName("FK_Ticket_ticketTypeId_ticketTypeId");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK_TicketType_ticketTypeId");

            entity.ToTable("TicketType");

            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeId");
            entity.Property(e => e.TicketName)
                .HasMaxLength(35)
                .HasColumnName("ticketName");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserAccount_userId");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Email, "UQ_UserAccount_email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAccount_roleId_roleId");
        });

        modelBuilder.Entity<UserInfor>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_UserInfor_staffId");

            entity.ToTable("UserInfor");

            entity.HasIndex(e => e.CitizenId, "UQ_UserInfor_citizenId").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.AccountStatus).HasColumnName("accountStatus");
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .HasColumnName("address");
            entity.Property(e => e.Bank)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("bank");
            entity.Property(e => e.BankAccount)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("bankAccount");
            entity.Property(e => e.BankAccountName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("bankAccountName");
            entity.Property(e => e.CitizenId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("citizenId");
            entity.Property(e => e.Country)
                .HasMaxLength(12)
                .HasColumnName("country");
            entity.Property(e => e.DepartmentId).HasColumnName("departmentId");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.FirstName)
                .HasMaxLength(15)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.HireDate)
                .HasColumnType("date")
                .HasColumnName("hireDate");
            entity.Property(e => e.LastName)
                .HasMaxLength(15)
                .HasColumnName("lastName");
            entity.Property(e => e.LeaveDayLeft).HasColumnName("leaveDayLeft");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Position)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("position");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.WorkTimeByYear).HasColumnName("workTimeByYear");

            entity.HasOne(d => d.Department).WithMany(p => p.UserInfors)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_UserInfor_departmentId_departmentId");

            entity.HasOne(d => d.User).WithMany(p => p.UserInfors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserInfor_userId_userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
