using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.UserInforDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UserInforController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        public UserInforController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserInforDto>>> GetUserInfors()
        {
            var staffs = await _context.UserInfors
                .ProjectUserInforToUserInforDto()
                .ToListAsync();
            return staffs;
        }

        [HttpGet("{id}", Name ="GetUserInforById")]
        public async Task<ActionResult<UserInforDto>> GetUserInforById(int id)
        {
            var staff = await _context.UserInfors
                .ProjectUserInforToUserInforDto()
                .FirstOrDefaultAsync(u => u.StaffId == id);
            
            if(staff == null) return NotFound();

            return staff;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveUserInfor(int id)
        {
            var staff = await _context.UserInfors
                .FirstOrDefaultAsync(u => u.StaffId == id);
            
            if(staff == null) return NotFound();

            staff.AccountStatus = false;

            _context.UserInfors.Update(staff);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails {Title = "Problem Removing User"});
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserInfor([FromBody] UserInforDto userInforDto)
        {
            if(userInforDto == null) return BadRequest("User data is missing");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var finalReturnUserInfor = _mapper.Map<UserInfor>(userInforDto);

            _context.UserInfors.Add(finalReturnUserInfor);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtAction(nameof(GetUserInforById), new {id = userInforDto.StaffId}, userInforDto);

            return BadRequest(new ProblemDetails {Title = "Problem adding user"});
        }

        [HttpPatch]
        public async Task<ActionResult<UserInfor>> UpdateDepartment(int id, [FromBody] UserInfor updatedUserInfor)
        {
            if(updatedUserInfor == null || updatedUserInfor.StaffId != id)
            {
                return BadRequest("Invalid Department Data");
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingUserInfor = await _context.UserInfors.FindAsync(id);

            if(existingUserInfor == null) return NotFound("Department Not Found");

            existingUserInfor.LastName = updatedUserInfor.LastName;
            existingUserInfor.FirstName = updatedUserInfor.FirstName;
            existingUserInfor.Dob = updatedUserInfor.Dob;
            existingUserInfor.Phone = updatedUserInfor.Phone;
            existingUserInfor.Gender = updatedUserInfor.Gender;
            existingUserInfor.Address = updatedUserInfor.Address;
            existingUserInfor.Country = updatedUserInfor.Country;
            existingUserInfor.DepartmentId = updatedUserInfor.DepartmentId;
            existingUserInfor.Position = updatedUserInfor.Position;
            existingUserInfor.BankAccount = updatedUserInfor.BankAccount;
            existingUserInfor.BankAccountName = updatedUserInfor.BankAccountName;
            existingUserInfor.Bank = updatedUserInfor.Bank;
            existingUserInfor.WorkTimeByYear = updatedUserInfor.WorkTimeByYear;
            existingUserInfor.AccountStatus = updatedUserInfor.AccountStatus;

            _context.UserInfors.Update(existingUserInfor);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok(existingUserInfor);

            return BadRequest(new ProblemDetails {Title = "Problem Update User"});
        }
    }
}