﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Entities;

public partial class SwpProjectContext : IdentityDbContext<User>
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

    public virtual DbSet<LeaveDayDetail> LeaveDayDetails { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<LogLeave> LogLeaves { get; set; }

    public virtual DbSet<LogOt> LogOts { get; set; }

    public virtual DbSet<OtDetail> OtDetails { get; set; }

    public virtual DbSet<OtType> OtTypes { get; set; }

    public virtual DbSet<Payslip> Payslips { get; set; }

    public virtual DbSet<PersonnelContract> PersonnelContracts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<StaffSkill> StaffSkills { get; set; }

    public virtual DbSet<TaxDetail> TaxDetails { get; set; }

    public virtual DbSet<TaxList> TaxLists { get; set; }

    public virtual DbSet<TheCalendar> TheCalendars { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserInfor> UserInfors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);database=swp_project;Uid=sa;Pwd=12345;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole{Name = "Staff", NormalizedName = "STAFF"},
                new IdentityRole{Name = "HRStaff", NormalizedName = "HRSTAFF"},
                new IdentityRole{Name = "HRManager", NormalizedName = "HRMANAGER"}
        );

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
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .HasColumnName("address");
            entity.Property(e => e.ApplyDate)
                .HasColumnType("date")
                .HasColumnName("applyDate");
            entity.Property(e => e.Department)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("department");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("email");
            entity.Property(e => e.ExpectedSalary).HasColumnName("expectedSalary");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.ImageFile)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("imageFile");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
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
                .HasName("PK__DateDime__AA552EF33AAFBED8")
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
                .HasMaxLength(35)
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

        modelBuilder.Entity<LeaveDayDetail>(entity =>
        {
            entity.HasKey(e => e.LeaveDayDetailId).HasName("PK_LeaveDayDetail_leaveDayDetailId");

            entity.ToTable("LeaveDayDetail");

            entity.Property(e => e.LeaveDayDetailId).HasColumnName("leaveDayDetailId");
            entity.Property(e => e.DayLeft).HasColumnName("dayLeft");
            entity.Property(e => e.LeaveTypeId).HasColumnName("leaveTypeId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveDayDetails)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_LeaveDayDetail_leaveTypeId_leaveTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.LeaveDayDetails)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_LeaveDayDetail_staffId_staffId");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId).HasName("PK_LeaveType_leaveTypeId");

            entity.ToTable("LeaveType");

            entity.Property(e => e.LeaveTypeId).HasColumnName("leaveTypeId");
            entity.Property(e => e.IsSalary).HasColumnName("isSalary");
            entity.Property(e => e.LeaveTypeDay).HasColumnName("leaveTypeDay");
            entity.Property(e => e.LeaveTypeDetail)
                .HasMaxLength(100)
                .HasColumnName("leaveTypeDetail");
            entity.Property(e => e.LeaveTypeName)
                .HasMaxLength(50)
                .HasColumnName("leaveTypeName");
        });

        modelBuilder.Entity<LogLeave>(entity =>
        {
            entity.HasKey(e => e.LeaveLogId).HasName("PK_LogLeave_leaveLogId");

            entity.ToTable("LogLeave");

            entity.Property(e => e.LeaveLogId).HasColumnName("leaveLogId");
            entity.Property(e => e.ChangeStatusTime)
                .HasColumnType("datetime")
                .HasColumnName("changeStatusTime");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.Description)
                .HasMaxLength(70)
                .HasColumnName("description");
            entity.Property(e => e.Enable).HasColumnName("enable");
            entity.Property(e => e.LeaveDays).HasColumnName("leaveDays");
            entity.Property(e => e.LeaveEnd)
                .HasColumnType("date")
                .HasColumnName("leaveEnd");
            entity.Property(e => e.LeaveHours).HasColumnName("leaveHours");
            entity.Property(e => e.LeaveStart)
                .HasColumnType("date")
                .HasColumnName("leaveStart");
            entity.Property(e => e.LeaveTypeId).HasColumnName("leaveTypeId");
            entity.Property(e => e.ProcessNote)
                .HasMaxLength(120)
                .HasColumnName("processNote");
            entity.Property(e => e.RespondencesId).HasColumnName("respondencesId");
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
            entity.Property(e => e.ChangeStatusTime)
                .HasColumnType("datetime")
                .HasColumnName("changeStatusTime");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.Enable).HasColumnName("enable");
            entity.Property(e => e.LogEnd)
                .HasColumnType("datetime")
                .HasColumnName("logEnd");
            entity.Property(e => e.LogHours).HasColumnName("logHours");
            entity.Property(e => e.LogStart)
                .HasColumnType("datetime")
                .HasColumnName("logStart");
            entity.Property(e => e.OtTypeId).HasColumnName("otTypeId");
            entity.Property(e => e.ProcessNote)
                .HasMaxLength(120)
                .HasColumnName("processNote");
            entity.Property(e => e.Reason)
                .HasMaxLength(25)
                .HasColumnName("reason");
            entity.Property(e => e.RespondencesId).HasColumnName("respondencesId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.OtType).WithMany(p => p.LogOts)
                .HasForeignKey(d => d.OtTypeId)
                .HasConstraintName("FK_LogOT_otTypeId_otTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.LogOts)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogOT_staffId_staffId");
        });

        modelBuilder.Entity<OtDetail>(entity =>
        {
            entity.HasKey(e => e.OtDetailId).HasName("PK_OtDetail_otDetailId");

            entity.ToTable("OtDetail");

            entity.Property(e => e.OtDetailId).HasColumnName("otDetailId");
            entity.Property(e => e.OtAmount).HasColumnName("otAmount");
            entity.Property(e => e.OtHours).HasColumnName("otHours");
            entity.Property(e => e.OtTypeId).HasColumnName("otTypeId");
            entity.Property(e => e.PayslipId).HasColumnName("payslipId");

            entity.HasOne(d => d.OtType).WithMany(p => p.OtDetails)
                .HasForeignKey(d => d.OtTypeId)
                .HasConstraintName("FK_OtDetail_otTypeId_otTypeId");

            entity.HasOne(d => d.Payslip).WithMany(p => p.OtDetails)
                .HasForeignKey(d => d.PayslipId)
                .HasConstraintName("FK_OtDetail_payslipId_payslipId");
        });

        modelBuilder.Entity<OtType>(entity =>
        {
            entity.HasKey(e => e.OtTypeId).HasName("PK_OtType_otTypeId");

            entity.ToTable("OtType");

            entity.Property(e => e.OtTypeId).HasColumnName("otTypeId");
            entity.Property(e => e.TypeName)
                .HasMaxLength(35)
                .HasColumnName("typeName");
            entity.Property(e => e.TypePercentage).HasColumnName("typePercentage");
        });

        modelBuilder.Entity<Payslip>(entity =>
        {
            entity.HasKey(e => e.PayslipId).HasName("PK_Payslip_payslipId");

            entity.ToTable("Payslip");

            entity.Property(e => e.PayslipId).HasColumnName("payslipId");
            entity.Property(e => e.ActualSalary).HasColumnName("actualSalary");
            entity.Property(e => e.ActualWorkDays).HasColumnName("actualWorkDays");
            entity.Property(e => e.BasicSalary).HasColumnName("basicSalary");
            entity.Property(e => e.Bhtncomp).HasColumnName("BHTNComp");
            entity.Property(e => e.Bhtnemp).HasColumnName("BHTNEmp");
            entity.Property(e => e.Bhxhcomp).HasColumnName("BHXHComp");
            entity.Property(e => e.Bhxhemp).HasColumnName("BHXHEmp");
            entity.Property(e => e.Bhytcomp).HasColumnName("BHYTComp");
            entity.Property(e => e.Bhytemp).HasColumnName("BHYTEmp");
            entity.Property(e => e.CreateAt)
                .HasColumnType("date")
                .HasColumnName("createAt");
            entity.Property(e => e.FamilyAllowances).HasColumnName("familyAllowances");
            entity.Property(e => e.GrossSalary).HasColumnName("grossSalary");
            entity.Property(e => e.LeaveDays).HasColumnName("leaveDays");
            entity.Property(e => e.LeaveHours).HasColumnName("leaveHours");
            entity.Property(e => e.NetSalary).HasColumnName("netSalary");
            entity.Property(e => e.OtTotal).HasColumnName("otTotal");
            entity.Property(e => e.PaiByDate).HasColumnName("paiByDate");
            entity.Property(e => e.PayslipStatus).HasColumnName("payslipStatus");
            entity.Property(e => e.PersonalIncomeTax).HasColumnName("personalIncomeTax");
            entity.Property(e => e.SalaryBeforeTax).HasColumnName("salaryBeforeTax");
            entity.Property(e => e.SalaryBonus).HasColumnName("salaryBonus");
            entity.Property(e => e.SalaryRecieved).HasColumnName("salaryRecieved");
            entity.Property(e => e.SalaryTaxable).HasColumnName("salaryTaxable");
            entity.Property(e => e.SelfAllowances).HasColumnName("selfAllowances");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.StandardWorkDays).HasColumnName("standardWorkDays");
            entity.Property(e => e.TotalAllowance).HasColumnName("totalAllowance");
            entity.Property(e => e.TotalInsured).HasColumnName("totalInsured");
            entity.Property(e => e.TotalPaid).HasColumnName("totalPaid");

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
            entity.Property(e => e.ContractFile)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("contractFile");
            entity.Property(e => e.ContractStatus).HasColumnName("contractStatus");
            entity.Property(e => e.ContractTypeId).HasColumnName("contractTypeId");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.NoOfDependences).HasColumnName("noOfDependences");
            entity.Property(e => e.Note)
                .HasMaxLength(50)
                .HasColumnName("note");
            entity.Property(e => e.PaidDateNote)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("paidDateNote");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.SalaryType)
                .HasMaxLength(10)
                .HasColumnName("salaryType");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
            entity.Property(e => e.TaxableSalary).HasColumnName("taxableSalary");
            entity.Property(e => e.WorkDatePerWeek).HasColumnName("workDatePerWeek");

            entity.HasOne(d => d.ContractType).WithMany(p => p.PersonnelContracts)
                .HasForeignKey(d => d.ContractTypeId)
                .HasConstraintName("FK_PersonnelContract_contractTypeId_contractTypeId");

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
                .HasMaxLength(50)
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

        modelBuilder.Entity<TaxDetail>(entity =>
        {
            entity.HasKey(e => e.TaxDetailId).HasName("PK_TaxDetail_taxDetailId");

            entity.ToTable("TaxDetail");

            entity.Property(e => e.TaxDetailId).HasColumnName("taxDetailId");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.PayslipId).HasColumnName("payslipId");
            entity.Property(e => e.TaxLevel).HasColumnName("taxLevel");

            entity.HasOne(d => d.Payslip).WithMany(p => p.TaxDetails)
                .HasForeignKey(d => d.PayslipId)
                .HasConstraintName("FK_TaxDetail_payslipId_payslipId");

            entity.HasOne(d => d.TaxLevelNavigation).WithMany(p => p.TaxDetails)
                .HasForeignKey(d => d.TaxLevel)
                .HasConstraintName("FK_TaxDetail_taxLevel_taxLevel");
        });

        modelBuilder.Entity<TaxList>(entity =>
        {
            entity.HasKey(e => e.TaxLevel).HasName("PK_TaxList_taxLevel");

            entity.ToTable("TaxList");

            entity.Property(e => e.TaxLevel).HasColumnName("taxLevel");
            entity.Property(e => e.Description)
                .HasMaxLength(40)
                .HasColumnName("description");
            entity.Property(e => e.TaxPercentage).HasColumnName("taxPercentage");
            entity.Property(e => e.TaxRange).HasColumnName("taxRange");
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
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.Enable).HasColumnName("enable");
            entity.Property(e => e.ProcessNote)
                .HasMaxLength(120)
                .HasColumnName("processNote");
            entity.Property(e => e.RespondencesId).HasColumnName("respondencesId");
            entity.Property(e => e.StaffId).HasColumnName("staffId");
            entity.Property(e => e.TicketFile)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ticketFile");
            entity.Property(e => e.TicketReason)
                .HasMaxLength(40)
                .HasColumnName("ticketReason");
            entity.Property(e => e.TicketStatus)
                .HasMaxLength(20)
                .HasColumnName("ticketStatus");
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
                .HasMaxLength(40)
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
            entity.Property(e => e.ImageFile)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("imageFile");
            entity.Property(e => e.IsManager).HasColumnName("isManager");
            entity.Property(e => e.LastName)
                .HasMaxLength(15)
                .HasColumnName("lastName");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.WorkTimeByYear).HasColumnName("workTimeByYear");

            entity.HasOne(d => d.Department).WithMany(p => p.UserInfors)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_UserInfor_departmentId_departmentId");

            entity.HasOne(d => d.User).WithOne(p => p.UserInfor)
                .HasForeignKey<UserInfor>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserInfor_userId_userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
