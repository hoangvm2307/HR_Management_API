using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.AllowanceDTO
{
    public class AllowanceCreationDTO
    {

        public int AllowanceTypeId { get; set; }

        public int AllowanceSalary { get; set; }
    }
}