﻿using API.DTOs.LogOtDTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class LogOtService
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LogOtService> _logger;
        private readonly TheCalendarService _theCalendarService;
        private readonly UserInfoService _userInfoService;
        private readonly PayslipService _payslipService;
        private readonly PersonnelContractService _personnelContractService;

        public LogOtService(
            SwpProjectContext context,
            IMapper mapper,
            ILogger<LogOtService> logger,
            TheCalendarService theCalendarService,
            UserInfoService userInfoService,
            //PayslipService payslipService,
            PersonnelContractService personnelContractService

            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _theCalendarService = theCalendarService ?? throw new ArgumentNullException(nameof(theCalendarService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            //_payslipService = payslipService ?? throw new ArgumentNullException(nameof(payslipService));
            _personnelContractService = personnelContractService ?? throw new ArgumentNullException(nameof(personnelContractService));
        }

        public async Task<List<LogOtDTO>> GetLogOts()
        {
            var logOtList = await _context.LogOts
                    .Include(c => c.Staff)
                    .Include(c => c.OtType)
                    .OrderByDescending(c => c.OtLogId)  
                    .ToListAsync();
            var returnLogOtList = _mapper.Map<List<LogOtDTO>>(logOtList);
            return returnLogOtList;
        }

        public async Task<List<LogOtDTO>> GetLogOtByStaffIdAsync(int StaffId)
        {
            var logOtList = await _context.LogOts
                .Include(c => c.Staff)
                .Include(c => c.OtType)
                .Where(c => c.StaffId == StaffId)
                    .OrderByDescending(c => c.OtLogId)  
                .ToListAsync();
            var returnLogOtList = _mapper.Map<List<LogOtDTO>>(logOtList);

            return returnLogOtList;
        }

        public async Task<LogOtDTO> GetLogOtById(int logOtId)
        {
            var logOt = await _context.LogOts
                    .Include(c => c.Staff)
                    .Include(c => c.OtType)
                    .OrderByDescending(c => c.OtLogId)  
                    .Where(c => c.OtLogId == logOtId)
                    .FirstOrDefaultAsync();

            var returnLogOt = _mapper.Map<LogOtDTO>(logOt);

            return returnLogOt;
        }

        public async Task CreateLogOT(int staffId, LogOtCreationDTO logOtCreationDTO)
        {

            var HolidayDates = await _theCalendarService.GetHolidayDates(logOtCreationDTO.LogStart, logOtCreationDTO.LogEnd);

            var WeekendDays = HolidayDates.Where(c => c.Percent == (decimal)2.0).ToList();
            var Holidays = HolidayDates.Where(c => c.Percent == (decimal)3.0).ToList();
            var HolidaysAndWeekends = HolidayDates.Where(c => c.Percent == (decimal)4.0).ToList();

            await AddLogOt(staffId, logOtCreationDTO, WeekendDays);
            await AddLogOt(staffId, logOtCreationDTO, Holidays);
            await AddLogOt(staffId, logOtCreationDTO, HolidaysAndWeekends);
          
            return;
        }

        public async Task AddLogOt(int staffId, LogOtCreationDTO logOtCreation, List<TheCalendar> theCalendar)
        {
            if (theCalendar == null)
            {
                return;
            }

            int type = getOtType(theCalendar.FirstOrDefault());

            if (type == 0)
            {
                return;
            }

            if(!await _userInfoService.IsUserExist(staffId))
            {
                return;
            }


            List<TheCalendar> list = new List<TheCalendar>();


            int length = theCalendar.Count;

            for (int i = 0; i < length; i++)
            {
                var current = theCalendar[i];
                var endPosition = i + 1;
                if (endPosition != length && theCalendar[i + 1].TheDay == current.TheDay + 1)
                {
                    list.Add(theCalendar[i]);
                }
                else if (endPosition != length && theCalendar[i + 1].TheDay != current.TheDay + 1)
                {
                    list.Add(theCalendar[i]);
                    await AddToDatabase(staffId, logOtCreation, type, list);

                    list.Clear();
                }
                else if (endPosition == length)
                {
                    list.Add(theCalendar[i]);
                    await AddToDatabase(staffId, logOtCreation, type, list);

                    list.Clear();
                }
            }
            await _context.SaveChangesAsync();

        }

        private async Task AddToDatabase(
            int staffId, 
            LogOtCreationDTO logOtCreation, 
            int type, 
            List<TheCalendar> list)
        {
            DateTime startDay = (DateTime)list.First().TheDate;
            DateTime endDay = (DateTime)list.Last().TheDate;

            var basicSalaryOfOneDay = await _personnelContractService
                .BasicSalaryOneDayOfMonth(staffId, startDay.Month, startDay.Year);

            var staffInfo = await GetUserInforEntityByStaffId(staffId);

            int percent = getPercent(type);
            int days = (endDay.Day - startDay.Day) + 1;

            logOtCreation.LogStart = startDay;
            logOtCreation.LogEnd = endDay;
            logOtCreation.OtTypeId = type;
            logOtCreation.LogHours = list.Count() * 8;
            logOtCreation.SalaryPerDay = basicSalaryOfOneDay * percent;
            logOtCreation.Days = days;
            logOtCreation.Amount = basicSalaryOfOneDay * percent * days;

            logOtCreation.CreateAt = DateTime.UtcNow.AddHours(7); 
            logOtCreation.ChangeStatusTime = DateTime.UtcNow.AddHours(7); 

            var WeekendsEntity = _mapper.Map<LogOt>(logOtCreation);

            staffInfo.LogOts.Add(WeekendsEntity);

            await _context.SaveChangesAsync();
        }

        public int getPercent(int otTypeId)
        {
            if (otTypeId == 1)
                return 2;
            else if (otTypeId == 2)
                return 3;
            else if (otTypeId == 3)
                return 4;
            return 0;
        }

        public int getOtType(TheCalendar theCalendar)
        {
            if (theCalendar == null)
            {
                return 0;
            }

            if (theCalendar.Percent ==  (decimal)2.0)
            {
                return 1;
            }
            else if (theCalendar.Percent == (decimal)3.0)
            {
                return 2;
            }
            else if(theCalendar.Percent == (decimal)4.0)
            {
                return 3;
            }

            return 0;
        }

        public async Task<bool> IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.UtcNow.AddHours(7);

            if (startDate < now || now > endDate)
            {
                return false;
            }
            else if (startDate > endDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       

        public async Task<bool> IsContainHoliday(DateTime start, DateTime end)
        {
            var IsContainHoliday = false;
            for(DateTime i = start; i <= end; i = i.AddDays(1))
            {
                IsContainHoliday = await _context.TheCalendars.AnyAsync(c => c.TheDate == i && c.Percent != 1);
                if(IsContainHoliday)
                {
                    break;
                }
            }
            return IsContainHoliday;
        }

        public async Task<bool> IsLogOtExist(int logOtId)
        {
            return await _context.LogOts.AnyAsync(c => c.OtLogId == logOtId);
        }

        public async Task<bool> IsDuplicateLogOt(int staffId, int? logOtId, DateTime startDate, DateTime endDate)
        {
            bool isDuplicate = false;

            // Check for duplicates within the entire date range of the LogOt object
            if (logOtId != null)
            {
                isDuplicate = await _context.LogOts.AnyAsync(c =>
                c.OtLogId != logOtId &&
                c.StaffId == staffId &&
                c.LogStart < endDate && c.LogEnd > startDate
                );
            }
            else if(logOtId == null)
            {
                isDuplicate = await _context.LogOts.AnyAsync(c =>
                c.StaffId == staffId &&
                c.LogStart < endDate && c.LogEnd > startDate
                );
            }

            return isDuplicate;
        }
        public async Task UpdateLogOt(int staffId, int logOtId, LogOtUpdateDTO logOtUpdateDTO)
        {
            var logOt =  await _context.LogOts
                                        .Where(c => c.StaffId == staffId && c.OtLogId == logOtId)
                                        .FirstOrDefaultAsync();


            _mapper.Map(logOtUpdateDTO, logOt);

            await _context.SaveChangesAsync();

        }

        public async Task<int> GetOtDays(int staffId, int month, int year)
        {
            var logOts = await _context.LogOts
                .Where(c => 
                    c.StaffId == staffId && 
                    c.LogStart.Month == month &&
                    c.LogStart.Year == year &&
                    c.Status == "approved")
                .ToListAsync();

            var logOtDays = logOts.Sum(c => c.LogHours) / 8;

            return (int)logOtDays;
        }

        public async Task<int> OtSalary(int staffId, int month, int year)
        {
            var logOts = await _context.LogOts
                .Where(c => 
                    c.StaffId == staffId && 
                    c.LogStart.Month == month && 
                    c.LogStart.Year == year &&
                    c.Status.ToLower().Equals("approved"))
                .ToListAsync();

            var OtSalary = logOts.Sum(c => c.Amount);

            if(OtSalary != null)
            {
                return (int)OtSalary;
            }

            return 0;
        }

        public async Task<UserInfor> GetUserInforEntityByStaffId(int StaffId)
        {
            return await _context.UserInfors
                                .Include(c => c.LogOts)
                                .Where(c => c.StaffId == StaffId && c.AccountStatus == true)
                                .FirstOrDefaultAsync();
        }
    }
}
