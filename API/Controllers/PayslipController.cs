﻿using API.DTOs.PayslipDTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using API.Extensions;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
        public async Task<ActionResult<List<PayslipDTO>>> GetPayslips()
        {
            return await _payslipService.GetPayslipAsync();
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
                return NotFound();
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
                return BadRequest(new ProblemDetails { Title= "Người dùng không có hợp đồng hợp lệ trong hệ thống, vui lòng kiểm tra lại thông tin hợp đồng", Status = 404});
            }

            var returnValue = await _payslipService.AddPayslipToDatabase(
                staffId, 
                payslipInputCreationDto.DateTime.Month,
                payslipInputCreationDto.DateTime.Year);


            return CreatedAtRoute(
                "GetPayslipByStaffIdAndPayslipId",
                new { payslipId = returnValue.PayslipId, staffId = returnValue.StaffId },
                returnValue);
        }

        [HttpPost("staffs")]
        public async Task<ActionResult<List<PayslipCreationDTO>>> CreatePayslipForAllStaff([FromBody] DateTime dateTime)
        {
            var staffIds = await _userInfoService.GetStaffIdsAsync();


            if (staffIds.Count == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Staff does not exist", Status = 404 });
            }

            foreach (var staffId in staffIds)
            {
                if (!await _personnelContractService.IsValidContractExist(staffId))
                {
                    return BadRequest(new ProblemDetails { Title = "Người dùng không có hợp đồng hợp lệ trong hệ thống, vui lòng kiểm tra lại thông tin hợp đồng", Status = 404 });
                }

                await _payslipService.AddPayslipToDatabase(
                staffId,
                dateTime.Month,
                dateTime.Year);
            }

            return NoContent();
        }

        [HttpPost("staffs/departments/{departmentId}")]
        public async Task<ActionResult<List<PayslipCreationDTO>>> CreatePayslipForDepartments(int departmentId, [FromBody] DateTime dateTime)
        {
            if (!await _departmentService.IsDepartmentExist(departmentId))
            {
                return BadRequest(new ProblemDetails { Title = "Department does not exist" , Status = 404});
            }

            var staffIds = await _userInfoService.GetStaffsOfDepartment(departmentId);

            if(staffIds.Count == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Staff does not exist", Status = 404 });
            }

            foreach (var staffId in staffIds)
            {
                if (!await _personnelContractService.IsValidContractExist(staffId))
                {
                    return BadRequest(new ProblemDetails { Title = "Người dùng không có hợp đồng hợp lệ trong hệ thống, vui lòng kiểm tra lại thông tin hợp đồng", Status = 404 });
                }

                var returnValue = await _payslipService.AddPayslipToDatabase(
                staffId,
                dateTime.Month,
                dateTime.Year);
            }
            return NoContent();
        }

    }
}