using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs.UserInforDTO
{
    public class UserInforDto
    {
        public int StaffId { get; set; }

        public int UserId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public DateTime? Dob { get; set; }

        public string? Phone { get; set; }

        public bool Gender { get; set; }

        public string? Address { get; set; }

        public string? Country { get; set; }

        public string CitizenId { get; set; } = null!;

        public int? DepartmentId { get; set; }

        public string? Position { get; set; }

        public DateTime HireDate { get; set; }

        public string BankAccount { get; set; } = null!;

        public string BankAccountName { get; set; } = null!;

        public string Bank { get; set; } = null!;

        public int? LeaveDayLeft { get; set; }

        public int? WorkTimeByYear { get; set; }

        public bool? AccountStatus { get; set; }

        // public virtual DepartmentDto Department { get; set; }
        
        // public LeaveDayLeftDto LeaveDayRemain { get; set; }

        // public List<LogLeaveDto> LogLeaves { get; set; }

        // public List<LogOtDto> LogOts { get; set; }

        // public List<PayslipDto> Payslips { get; set; }

        // public List<PersonnelContractDto> PersonnelContracts { get; set; }

        // public List<StaffSkillDto> StaffSkills { get; set; }

        // public List<TicketDto> Tickets { get; set; }

        // public UserAccountDto User { get; set; }
    }
}