using API.DTOs.LogLeaveDTO;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Services
{
    public class LogLeaveService
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly TheCalendarService _theCalendarService;
        private readonly UserInfoService _userInfoService;
        private readonly ILogger _logger;

        public LogLeaveService(
            SwpProjectContext context,
            IMapper mapper,
            TheCalendarService theCalendarService,
            UserInfoService userInfoService,
            ILogger<LogLeaveService> logger
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _theCalendarService = theCalendarService ?? throw new ArgumentNullException(nameof(theCalendarService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<LogLeaveDTO>> GetLogLeaveDTOs()
        {
            var logleaves = await _context.LogLeaves
                                    .Include(c => c.LeaveType)
                                    .Include(c => c.Staff)
                                    .ToListAsync();

            var returnLogLeaves = _mapper.Map<List<LogLeaveDTO>>(logleaves);
            return returnLogLeaves;
        }
        public async Task<List<LogLeaveDTO>> GetLogLeavesByStaffId(int staffId)
        {
            var logLeaves = await _context.LogLeaves
                                    .Include(c => c.LeaveType)
                                    .Include(c => c.Staff)
                                    .Where(c => c.StaffId == staffId)
                                    .ToListAsync();

            var returnLogLeaves = _mapper.Map<List<LogLeaveDTO>>(logLeaves);
            return returnLogLeaves;
        }

        public async Task<bool> IsLogLeaveExist(int logLeaveId)
        {
            return await _context.LogLeaves.AnyAsync(c => c.LeaveLogId == logLeaveId);
        }

        public async Task<LogLeave> GetLogLeaveAsync(int staffId, int logLeaveId)
        {
            return await _context.LogLeaves
                                       .Include(c => c.LeaveType)
                                       .Include(c => c.Staff)
                                       .Where(c => c.StaffId == staffId && c.LeaveLogId == logLeaveId)
                                       .FirstOrDefaultAsync();
        }

        public async Task<LogLeaveDTO> GetLogLeaveByStaffIdAndLogLeaveId(int staffId, int logLeaveId)
        {
            var logLeave = await _context.LogLeaves
                                        .Include(c => c.LeaveType)
                                        .Include(c => c.Staff)
                                        .Where(c => c.StaffId == staffId && c.LeaveLogId == logLeaveId)
                                        .FirstOrDefaultAsync();
            var returnLogLeave = _mapper.Map<LogLeaveDTO>(logLeave);
            return returnLogLeave;
        }
        public async Task UpdateLogLeave(int staffId, int logLeaveId, LogLeaveUpdateDTO logLeaveUpdateDTO)
        {
            var logLeave = await _context.LogLeaves
                                        .Include(c => c.LeaveType)
                                        .Include(c => c.Staff)
                                        .Where(c => c.StaffId == staffId && c.LeaveLogId == logLeaveId)
                                        .FirstOrDefaultAsync();

            if (logLeave == null)
            {
                return;
            }

            _mapper.Map(logLeaveUpdateDTO, logLeave);

            await _context.SaveChangesAsync();

        }

        public async Task<UserInfor> GetUserLogLeave(int staffId)
        {
            var user = await _context.UserInfors
                .Include(c => c.LogLeaves)
                .Where(c => c.StaffId == staffId)
                .FirstOrDefaultAsync();
            return user;
        }
        public async Task<int> LeaveDaysCalculation(DateTime start, DateTime end)
        {
            int leaveDays = 0;
            for(var i = start; i <= end; i = i.AddDays(1))
            {
                if(await _context.TheCalendars
                    .Where(c =>
                        c.IsWorking == 1 &&
                        c.TheDay == i.Day &&
                        c.TheMonth == i.Month &&
                        c.TheYear == i.Year
                        )
                    .AnyAsync())
                    leaveDays++;
            }

            return leaveDays;
        }

        public async Task<int> GetLeaveDays(int staffId, int month, int year)
        {
            var logLeaves = await _context.LogLeaves
                    .Where(c =>
                        c.StaffId == staffId &&
                        c.Status == "approved" &&
                        month >= c.LeaveStart.Month &&
                        month <= c.LeaveEnd.Month
                        )
                    .ToListAsync();

            int logLeavesDays = 0;

            foreach (var item in logLeaves)
            {
                int startDay = GetStartDay(month, item.LeaveStart);
                int endDay = await GetEndDay(month, year, item.LeaveEnd);

                logLeavesDays += (endDay - startDay) + 1;
            }

            return logLeavesDays;
        }

        public async Task<int> GetLeavesHours(int staffId, int month, int year)
        {
            var logLeaves = await _context.LogLeaves
                    .Where(c =>
                        c.StaffId == staffId &&
                        c.Status == "approved" &&
                        month >= c.LeaveStart.Month &&
                        month <= c.LeaveEnd.Month
                        )
                    .ToListAsync();

            int leaveHours = 0;

            foreach (var item in logLeaves)
            {
                int startDay = GetStartDay(month, item.LeaveStart);
                int endDay = await GetEndDay(month, year, item.LeaveEnd);

                leaveHours += (endDay - startDay) + 1;
            }

            return leaveHours * 8;
        }

        public int GetStartDay(int month, DateTime start)
        {
            if (month < 1 || month > 12)
            {
                _logger.LogCritical("error at month");
                return 0;
            }
            if(month < start.Month)
            {
                _logger.LogCritical("error at month");
                return 0;
            }

            if (month == start.Month)
            {
                _logger.LogCritical(start.Day.ToString());
                return start.Day;
            }
            else
            {
                _logger.LogCritical(1.ToString());

                return 1;
            }
        }

        public async Task<int> GetEndDay(int month, int year, DateTime end)
        {
            if (month < 1 || month > 12)
            {
                _logger.LogCritical("error at month");
                return 0;
            }

            if (month > end.Month)
            {
                _logger.LogCritical("error at month");
                return 0;
            }

            if (month == end.Month)
            {
                _logger.LogCritical(end.Day.ToString());
                return end.Day;
            }
            else
            {
                var lastDayOfMonth = await _context.TheCalendars
                                     .Where(c =>
                                        c.TheMonth == month &&
                                        c.TheYear == year)
                                    .OrderByDescending(c => c.TheDay)
                                    .FirstOrDefaultAsync();

                var lastDay = lastDayOfMonth.TheDay;

                _logger.LogCritical("End Day: " + lastDay.ToString());

                return (int)lastDay;
            }
        }

        public async Task DemoDate(int month, DateTime start, DateTime end)
        {
            var year = 2023;


            GetStartDay(month, start);

            await GetEndDay(month, year, end);
        }

        public async Task<LogLeaveDTO> CreateLogLeave(int staffId, LogLeaveCreationDTO logLeaveCreationDTO)
        {

            var user = await GetUserLogLeave(staffId);

            var logLeave = _mapper.Map<LogLeave>(logLeaveCreationDTO);

            user.LogLeaves.Add(logLeave);

            await _context.SaveChangesAsync();


            var returnLogLeave = _mapper.Map<LogLeaveDTO>(logLeave);

            return returnLogLeave;
        }
    }
}
