using API.DTOs.LoginDTO;
using API.DTOs.RegisterDTO;
using API.DTOs.UserDTO;
using API.DTOs.UserInforDTO;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly SwpProjectContext _context;
        public AccountController(UserManager<User> userManager, TokenService tokenService, IMapper mapper, SwpProjectContext context)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager
                .FindByNameAsync(loginDto.Username);

            if(user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            var userInfor = _context.UserInfors.FirstOrDefault(c => c.Id == user.Id);

            var userInforDto = _mapper.Map<UserInforDto>(userInfor);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                UserInfor = userInforDto
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if(!_context.Departments.Any()) return NotFound("No Departments");

            var user = new User {UserName = registerDto.Username, Email = registerDto.Email};
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Staff");

            var userInfor = new UserInfor
            {
                Id = user.Id,
                LastName = registerDto.LastName,
                FirstName = registerDto.FirstName,
                Dob = registerDto.Dob,
                Phone = registerDto.Phone,
                Gender = registerDto.Gender,
                Address = registerDto.Address,
                Country = registerDto.Country,
                CitizenId = registerDto.CitizenId,
                DepartmentId = registerDto.DepartmentId,
                IsManager = registerDto.IsManager,
                HireDate = DateTime.Now,
                BankAccount = registerDto.BankAccount,
                BankAccountName = registerDto.BankAccountName,
                Bank = registerDto.Bank,
                WorkTimeByYear = 0,
                AccountStatus = true
            };

            _context.UserInfors.Add(userInfor);

            if(await _context.SaveChangesAsync() > 0) return StatusCode(201);

            _userManager.DeleteAsync(user);

            return BadRequest();

             
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var userInfor = _context.UserInfors.FirstOrDefault(c => c.Id == user.Id);

            var userInforDto = _mapper.Map<UserInforDto>(userInfor);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                UserInfor = userInforDto
            };
        }
    }
}