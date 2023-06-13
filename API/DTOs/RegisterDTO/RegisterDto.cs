using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.LoginDTO;
using API.DTOs.UserInforDTO;

namespace API.DTOs.RegisterDTO
{
    public class RegisterDto : LoginDto
    {
        public string Email { get; set; }

        // public UserInforDto UserInforDto {get;set;}

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public DateTime? Dob { get; set; }

        public string? Phone { get; set; }

        public bool Gender { get; set; }

        public string? Address { get; set; }

        public string? Country { get; set; }

        public string CitizenId { get; set; } = null!;

        public int? DepartmentId { get; set; }

        public bool? IsManager { get; set; }

        public string? BankAccount { get; set; }

        public string? BankAccountName { get; set; } = null!;

        public string? Bank { get; set; } = null!;

    }
}