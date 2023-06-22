using API.DTOs.UserInforDTO;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class UserInforController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserInforController(SwpProjectContext context, IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserInforDto>>> GetUserInfors()
        {
            var userInfors = await _context.UserInfors
                .ToListAsync();
            
            var userInforDtos = _mapper.Map<List<UserInforDto>>(userInfors);
            
            userInforDtos = userInforDtos.Select(userInforDto => 
            {
                userInforDto.Email = GetUserEmailByIdAsync(userInforDto.Id).Result;
                userInforDto.Position = userInforDto.IsManager ? "Manager" : "Staff";

                userInforDto.DepartmentName = GetDepartmentNameByIdAsync
                    (userInforDto.DepartmentId ?? 0).Result;
                return userInforDto;
            }).ToList();

            return userInforDtos;
        }

        [HttpGet("{id}", Name ="GetUserInforById")]
        public async Task<ActionResult<UserInforDto>> GetUserInforById(int id)
        {
            var userInfor = await _context.UserInfors
                .FirstOrDefaultAsync(u => u.StaffId == id);
        
            if(userInfor == null) return NotFound();

            var userInforDto = _mapper.Map<UserInforDto>(userInfor);

            userInforDto.Email = GetUserEmailByIdAsync(userInforDto.Id).Result;
            userInforDto.Position = userInforDto.IsManager ? "Manager" : "Staff";

            return userInforDto;
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

        // [HttpPost]
        // public async Task<ActionResult> CreateUserInfor([FromBody] UserInforDto userInforDto)
        // {
        //     var userInfor = new UserInfor
        //     {
        //         Id = user.Id,
        //         LastName = userInforDto.LastName,
        //         FirstName = userInforDto.FirstName,
        //         Dob = userInforDto.Dob,
        //         Gender = userInforDto.Gender,
        //         Address = userInforDto.Address,
        //         Country = userInforDto.Country,
        //         CitizenId = userInforDto.CitizenId,
        //         DepartmentId = userInforDto.DepartmentId,
        //         Position = userInforDto.Position,
        //         HireDate = userInforDto.HireDate,
        //         BankAccount = userInforDto.BankAccount,
        //         BankAccountName = userInforDto.BankAccountName,
        //         Bank = userInforDto.Bank,
        //         WorkTimeByYear = userInforDto.WorkTimeByYear,
        //         AccountStatus = userInforDto.AccountStatus
        //     };
        //     _context.UserInfors.Add(userInfor);
        //     if(await _context.SaveChangesAsync() > 0)
        //     {
        //         return StatusCode(201);
        //     }

        //     if(result) return CreatedAtAction(nameof(GetUserInforById), new {id = userInforDto.StaffId}, userInforDto);

        //     return BadRequest(new ProblemDetails {Title = "Problem adding user"});
        // }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUserInfor(int id, [FromBody] JsonPatchDocument<UserInforDto> patchDocument)
        {
            var userInfor = await _context.UserInfors.FindAsync(id);

            if (userInfor == null)
            {
                return NotFound();
            }

            var userInforDto = _mapper.Map<UserInforDto>(userInfor);

            patchDocument.ApplyTo(userInforDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(userInforDto, userInfor);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<string> GetUserEmailByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }
        private async Task<string> GetDepartmentNameByIdAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id);
            return department?.DepartmentName;
        }
    }
}