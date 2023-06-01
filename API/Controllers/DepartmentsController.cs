using API.DTOs.DepartmentDTO;
using API.Entities;
using API.Extensions;
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
        public async Task<ActionResult<List<DepartmentDto>>> GetDepartmentss()
        {
            var departments = await _context.Departments
            .ProjectDepartmentToDepartmentDto()
            .ToListAsync();
            return departments;
        }

        [HttpGet("{id}", Name="GetDepartment")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var department = await _context.Departments
                .ProjectDepartmentToDepartmentDto()
                .FirstOrDefaultAsync(d => d.DepartmentID == id);
            return department;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveDepartment(int departmentId)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
            
            if(department == null) return NotFound();

            _context.Departments.Remove(department);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails {Title = "Problem removing"});
        }

        [HttpPost]
        public async Task<ActionResult> CreateDepartment([FromBody] Department department)
        {
            if(department == null) return BadRequest("Department data is missing");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _context.Departments.Add(department);
            
            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetDepartment), new {id = department.DepartmentId}, department);

            return BadRequest(new ProblemDetails {Title = "Problem adding item"});
        }
    }
}