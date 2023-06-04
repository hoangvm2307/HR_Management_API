using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.LoginDTO;

namespace API.DTOs.RegisterDTO
{
    public class RegisterDto : LoginDto
    {
        public string Email { get; set; }
    }
}