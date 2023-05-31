using API.DTOs.DepartmentDTO;
using API.Extensions;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class DepartmentsController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        public DepartmentsController(SwpProjectContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<DepartmentDTO>>> GetDepartments()
        {
            var departments = await _context.Departments
            .ProjectDepartmentToDepartmentDTO()
            .ToListAsync();
            return departments;
        }
        [HttpGet("{id}", Name="GetDepartment")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            var department = await _context.Departments
                .ProjectDepartmentToDepartmentDTO()
                .FirstOrDefaultAsync(d => d.DepartmentID == id);
            return department;
        }
    }
}