using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.DepartmentDTO
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string Manager { get; set; }

        public int numberOfStaff { get; set; }

        public string ManagerPhone { get; set; }

        public string ManagerMail { get; set; }

        public List<DepartmentUserInforDto> UserInfors {get;set;}
    }
}