using API.DTOs.DepartmentDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class DepartmentsController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public DepartmentsController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartmentDto>>> GetDepartments()
        {
            var departments = await _context.Departments
            .Include(i => i.UserInfors)
            .ToListAsync();

            var returnDepartments = _mapper.Map<List<DepartmentDto>>(departments);
            return returnDepartments;
        }
        
        // [HttpGet]
        // public async Task<ActionResult<List<DepartmentUserInforDto>>> GetDepartmentss()
        // {
        //     var departments = await _context.UserInfors
        //     .Include(i => i.Department)
        //     .ToListAsync();

        //     var returnDepartments = _mapper.Map<List<DepartmentUserInforDto>>(departments);

        //     return returnDepartments;
        // }

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
        public async Task<ActionResult> CreateDepartment(DepartmentCreateDto departmentDto)
        {
            if(departmentDto == null) return BadRequest("Department data is missing");
            
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var department = new Department
            {
                DepartmentName = departmentDto.DepartmentName,
            };
        
            if(departmentDto.ManagerId != 0)
            {
                var userInfor = _context.UserInfors
                    .FirstOrDefault(c => c.StaffId == departmentDto.ManagerId);
                userInfor.IsManager = true;
            }
             
            _context.Departments.Add(department);
            
            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetDepartment), new {id = department.DepartmentId}, department);

            return BadRequest(new ProblemDetails {Title = "Problem adding item"});
        }

        [HttpPut]
        public async Task<ActionResult<Department>> UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            if(updatedDepartment == null || updatedDepartment.DepartmentId != id)
            {
                return BadRequest("Invalid Department Data");
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingDepartment = await _context.Departments.FindAsync(id);

            if(existingDepartment == null) return NotFound("Department Not Found");

            existingDepartment.DepartmentName = updatedDepartment.DepartmentName;

            _context.Departments.Update(existingDepartment);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingDepartment);

            return BadRequest(new ProblemDetails {Title = "Problem Update Department"});
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDepartment(int id, [FromBody] JsonPatchDocument<DepartmentUpdateDto> patchDocument)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            var departmentDto = _mapper.Map<DepartmentUpdateDto>(department);

            patchDocument.ApplyTo(departmentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(departmentDto, department);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return NoContent();

            return BadRequest("Problem Updateing Department");
        }
    }
}