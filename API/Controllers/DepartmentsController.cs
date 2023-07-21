using API.DTOs.DepartmentDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
  public class DepartmentsController : BaseApiController
  {
    private readonly SwpProjectContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    public DepartmentsController(SwpProjectContext context, IMapper mapper, UserManager<User> userManager)
    {
      _mapper = mapper;
      _context = context;
      _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentDto>>> GetDepartments()
    {
      var departments = await _context.Departments
          .Include(i => i.UserInfors)
          .ToListAsync();

      var returnDepartments = _mapper.Map<List<DepartmentDto>>(departments);

      returnDepartments = returnDepartments.Select(departmentDto =>
      {
        var manager = departmentDto.UserInfors.FirstOrDefault(u => u.IsManager == true);

        if (manager != null)
        {
          departmentDto.ManagerId = manager.StaffId;
          departmentDto.Manager = $"{manager.FirstName} {manager.LastName}";
          departmentDto.ManagerPhone = $"{manager.Phone}";
          departmentDto.ManagerMail = GetUserEmailByIdAsync(manager.Id).Result;
        }

        departmentDto.UserInfors = departmentDto.UserInfors.Select(userInfor =>
              {
                userInfor.Email = GetUserEmailByIdAsync(userInfor.Id).Result;
                userInfor.Position = userInfor.IsManager ? "Manager" : "Staff";
                return userInfor;
              }).ToList();

        departmentDto.numberOfStaff = departmentDto.UserInfors.Count;

        return departmentDto;
      }).ToList();

      return returnDepartments;
    }


    [HttpGet("{id}", Name = "GetDepartment")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
      var department = await _context.Departments
          .Include(i => i.UserInfors)
          .FirstOrDefaultAsync(d => d.DepartmentId == id);

      var departmentDto = _mapper.Map<DepartmentDto>(department);

      departmentDto.UserInfors = departmentDto.UserInfors.Select(userInfor =>
              {
                userInfor.Email = GetUserEmailByIdAsync(userInfor.Id).Result;
                userInfor.Position = userInfor.IsManager ? "Manager" : "Staff";
                return userInfor;
              }).ToList();

      var manager = departmentDto.UserInfors.FirstOrDefault(u => u.IsManager == true);

      if (manager != null)
      {
        departmentDto.ManagerId = manager.StaffId;
        departmentDto.Manager = $"{manager.FirstName} {manager.LastName}";
        departmentDto.ManagerPhone = $"{manager.Phone}";
        departmentDto.ManagerMail = await GetUserEmailByIdAsync(manager.Id);
      }

      departmentDto.numberOfStaff = departmentDto.UserInfors.Count;

      return departmentDto;
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveDepartment(int departmentId)
    {
      var department = await _context.Departments
          .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

      if (department == null) return NotFound();

      _context.Departments.Remove(department);

      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok();

      return BadRequest(new ProblemDetails { Title = "Problem removing" });
    }


    [HttpPost]
    public async Task<ActionResult> CreateDepartment(DepartmentCreateDto departmentDto)
    {
      if (departmentDto == null) return BadRequest("Department data is missing");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var department = new Department
      {
        DepartmentName = departmentDto.DepartmentName,
      };

      if (departmentDto.ManagerId != 0)
      {
        var userInfor = _context.UserInfors
            .FirstOrDefault(c => c.StaffId == departmentDto.ManagerId);
        userInfor.IsManager = true;
      }

      _context.Departments.Add(department);

      await _context.SaveChangesAsync();

      if (departmentDto.UserInfors.Any())
      {
        var userInfors = await _context.UserInfors
            .Where(u => departmentDto.UserInfors.Select(d => d).Contains(u.StaffId))
            .ToListAsync();

        userInfors = userInfors.Select(userInfor =>
        {
          userInfor.DepartmentId = department.DepartmentId;

          return userInfor;
        }).ToList();
      }

      var result = await _context.SaveChangesAsync() > 0;

      return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Department>> UpdateDepartment(int id, DepartmentUpdateDto departmentDto)
    {
      if (departmentDto == null)
      {
        return BadRequest("Invalid Department Data");
      }

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var existingDepartment = await _context.Departments
          .Include(i => i.UserInfors)
          .FirstOrDefaultAsync(d => d.DepartmentId == id);

      if (existingDepartment == null) return NotFound("Department Not Found");

      var existingDepartmentDto = _mapper.Map<DepartmentDto>(existingDepartment);

      if (!string.IsNullOrWhiteSpace(departmentDto.DepartmentName))
        existingDepartmentDto.DepartmentName = departmentDto.DepartmentName;

      if (departmentDto.UserInfors.Any())
      {
        var userInfors = await _context.UserInfors
            .Where(u => departmentDto.UserInfors.Select(d => d.StaffId).Contains(u.StaffId))
            .ToListAsync();

        userInfors = userInfors.Select(userInfor =>
        {
          userInfor.DepartmentId = id;
          userInfor.IsManager = false;
          return userInfor;
        }).ToList();

        _context.Departments.Update(_mapper.Map<Department>(existingDepartment));
      }

      // Retrieve the new manager information
      if (departmentDto.ManagerId != 0)
      {
        var newManager = await _context.UserInfors
            .FirstOrDefaultAsync(u => u.StaffId == departmentDto.ManagerId);

        if (newManager != null)
        {
          // Remove the previous manager flag from the old manager
          // var oldManager = existingDepartmentDto.UserInfors
          //    .FirstOrDefault(u => u.IsManager == true);

          var oldManager = await _context.UserInfors
              .FirstOrDefaultAsync(u => u.DepartmentId == id && u.IsManager == true);

          if (oldManager != null) oldManager.IsManager = false;
        }

        // Set the new manager flag in the new manager's UserInfor object
        var newManagerUserInfor = _context.UserInfors
            .FirstOrDefault(ui => ui.StaffId == newManager.StaffId);

        if (newManagerUserInfor != null) newManagerUserInfor.IsManager = true;
      }


      var result = await _context.SaveChangesAsync() > 0;

      if (result) return Ok(existingDepartment);

      return BadRequest(new ProblemDetails { Title = "Problem Update Department" });
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

      return NoContent();
    }
    private async Task<string> GetUserEmailByIdAsync(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      return user?.Email;
    }
  }
}