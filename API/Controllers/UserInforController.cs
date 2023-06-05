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

        // [HttpGet("{id}", Name ="GetUserInforById")]
        // public async Task<ActionResult<UserInforDto>> GetUserInforById(int id)
        // {
        //     var staff = await _context.UserInfors
        //         .ProjectUserInforToUserInforDto()
        //         .FirstOrDefaultAsync(u => u.StaffId == id);
            
        //     if(staff == null) return NotFound();

        //     return staff;
        // }

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

//         [HttpPatch]
//         public async Task<ActionResult<UserInforDto>> PartiallyUpdateUserInfor(
// int StaffId,
//             JsonPatchDocument<LogOtUpdateDTO> patchDocument
//         )
//         {

//             var staffInfo = await _context.UserInfors.Where(c => c.StaffId == StaffId).FirstOrDefaultAsync();

//             if (staffInfo == null)
//             {
//                 return NotFound();
//             }
//             var LogOtFromStore = await _context.LogOts.Where(c => c.StaffId == StaffId && c.OtLogId == LogOtId).FirstOrDefaultAsync();

//             if (LogOtFromStore == null)
//             {
//                 return NotFound();
//             }

//             var logOtpath = _mapper.Map<LogOtUpdateDTO>(LogOtFromStore);

//             patchDocument.ApplyTo(logOtpath,ModelState);

//             if (!ModelState.IsValid)
//             {
//                 return BadRequest(ModelState);
//             }

//             if (!TryValidateModel(logOtpath))
//             {
//                 return BadRequest(ModelState);
//             }

//             _mapper.Map(logOtpath, LogOtFromStore);
//             await _context.SaveChangesAsync();

//             return NoContent();   
//         }
    }
}