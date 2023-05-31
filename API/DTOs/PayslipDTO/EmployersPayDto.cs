using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class EmployersPayDto : InsuranceDto
    {
        public decimal GrossSalary { get; set; }
        public decimal Total { get; set; }
    }
}