﻿using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using API.Extensions;
using API.RequestHelpers;
using API.Services;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/payslips")]
    public class PayslipController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly PayslipService _payslipService;
        private readonly UserInfoService _userInfoService;
        private readonly PersonnelContractService _personnelContractService;
        private readonly LogLeaveService _logLeaveService;
        private readonly DepartmentService _departmentService;

        public PayslipController(
            SwpProjectContext context,
            IMapper mapper,
            PayslipService payslipService,
            UserInfoService userInfoService,
            PersonnelContractService personnelContractService,
            LogLeaveService logLeaveService,
            DepartmentService departmentService
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _payslipService = payslipService ?? throw new ArgumentNullException(nameof(payslipService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
            _logLeaveService = logLeaveService ?? throw new ArgumentNullException(nameof(logLeaveService));
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }


        [HttpGet]
        public async Task<ActionResult<PagedList<PayslipDTO>>> GetPayslips(
            [FromQuery]PayslipParams payslipParams
            )
        {
            var payslips = await _payslipService.GetPayslipAsync(payslipParams);

            Response.AddPaginationHeader(payslips.MetaData);

            return payslips;

        }

        

        [HttpGet("{staffId}")]
        public async Task<ActionResult<List<PayslipDTO>>> GetPayslipListByStaffId(int staffId)
        {
            return await _payslipService.GetPayslipOfStaff(staffId);
        }

        [HttpGet("{payslipId}/staffs/{staffId}", Name = "GetPayslipByStaffIdAndPayslipId")]
        public async Task<ActionResult<PayslipDTO>> GetPayslipByStaffIdAndPayslipId(int staffId, int payslipId)
        {
            if(!await _payslipService.IsPayslipExist(staffId, payslipId))
            {
                return BadRequest(new ProblemDetails { Title = "Bảng lương không tồn tại"});
            }
           
            return await _payslipService.GetPayslipOfStaffByPayslipId(staffId, payslipId);

        }

        [HttpPost("staffs/{staffId}")]
        public async Task<ActionResult<PayslipDTO>> CreatePayslipByStaffIdForAMonth(
            int staffId,
            PayslipInputCreationDto payslipInputCreationDto
            )
        {
            if (!await _userInfoService.IsUserExist(staffId))
            {
                return NotFound();
            }

            if (!await _personnelContractService.IsValidContractExist(staffId))
            {
                return BadRequest(new ProblemDetails { Title = "Người dùng không có hợp đồng hợp lệ trong hệ thống, vui lòng kiểm tra lại thông tin hợp đồng" });
            }

            var returnValue = await _payslipService.AddPayslipToDatabase(
                staffId, 
                payslipInputCreationDto);


            return CreatedAtRoute(
                "GetPayslipByStaffIdAndPayslipId",
                new { payslipId = returnValue.PayslipId, staffId = returnValue.StaffId },
                returnValue);
        }

        [HttpPost("staffs")]
        public async Task<ActionResult<List<PayslipCreationDTO>>> CreatePayslipForAllStaff(PayslipInputCreationDto payslipInputCreationDto)
        {
            var staffIds = await _userInfoService.GetStaffIdsAsync();


            if (staffIds.Count == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Nhân viên không tồn tại"});
            }

            foreach (var staffId in staffIds)
            {
                if (!await _personnelContractService.IsValidContractExist(staffId))
                {
                    return BadRequest(new ProblemDetails { Title = "Người dùng không có hợp đồng hợp lệ trong hệ thống"});
                }

                await _payslipService.AddPayslipToDatabase(
                staffId,
                payslipInputCreationDto);
            }

            return NoContent();
        }

        [HttpPost("departments/{departmentId}")]
        public async Task<ActionResult<List<PayslipCreationDTO>>> CreatePayslipForDepartments(int departmentId, PayslipInputCreationDto  payslipInputCreationDto)
        {
            if (!await _departmentService.IsDepartmentExist(departmentId))
            {
                return BadRequest(new ProblemDetails { Title = "Phòng ban không tồn tại" });
            }

            var staffIds = await _userInfoService.GetStaffsOfDepartment(departmentId);

            if(staffIds.Count == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Nhân viên không tồn tại"});
            }

            foreach (var staffId in staffIds)
            {
                if (!await _personnelContractService.IsValidContractExist(staffId))
                {
                    return BadRequest(new ProblemDetails { Title = "Người dùng không có hợp đồng hợp lệ trong hệ thống"});
                }

                var returnValue = await _payslipService.AddPayslipToDatabase(
                staffId,
                payslipInputCreationDto);
            }
            return NoContent();
        }

        [HttpPut("{payslipId}")]
        public async Task<ActionResult<PayslipDTO>> UpdatePayslip(
            int payslipId, 
            PayslipUpdateDTO payslipUpdateDTO)
        {
            if(!await _payslipService.IsPayslipExist(payslipId))
            {
                return BadRequest(new ProblemDetails { Title = "Bảng lương không tồn tại"});
            }

            var payslip = await _context.Payslips
                .Where(c => c.PayslipId == payslipId)
                .FirstOrDefaultAsync();
            payslipUpdateDTO.ChangeAt = DateTime.UtcNow.AddHours(7);
            _mapper.Map(payslipUpdateDTO, payslip);

            await _context.SaveChangesAsync();
            

            return NoContent();
        }

        [HttpPatch("{payslipId}")]
        public async Task<ActionResult<PayslipDTO>> PartialUpdatePayslip(
            int payslipId,
            JsonPatchDocument<PayslipUpdateDTO> jsonPatchDocument)
        {
            if (!await _payslipService.IsPayslipExist(payslipId))
            {
                return BadRequest(new ProblemDetails { Title = "Bảng lương không tồn tại" });
            }

            var payslip = await _context.Payslips
                .Where(c => c.PayslipId == payslipId)
                .FirstOrDefaultAsync();
            
            if(payslip == null)
            {
                return BadRequest(new ProblemDetails { Title = "Bảng lương không tồn tại" });

            }

            payslip.ChangeAt = DateTime.UtcNow.AddHours(7);

            var payslipPatch = _mapper.Map<PayslipUpdateDTO>(payslip);

            jsonPatchDocument.ApplyTo(payslipPatch, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            if (!TryValidateModel(payslipPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(payslipPatch, payslip);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilter()
        {
            var departments = await _context.Payslips
                .Include(c => c.Staff)
                .ThenInclude(c => c.Department)
                .Select(c => c.Staff.Department.DepartmentName)
                .Distinct()
                .ToListAsync();
            return Ok(departments);
        }
    }
}