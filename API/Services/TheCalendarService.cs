using API.DTOs.CalendarDto;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TheCalendarService
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public TheCalendarService(SwpProjectContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TheCalendar>> GetHolidayDates(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.Now;

            List<TheCalendar> holidayDimensions = new List<TheCalendar>();
            if (startDate > endDate)
            {
                return holidayDimensions.ToList();
            }

            for(DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dateType = await _context.TheCalendars.Where(c => c.Percent != 1 && c.TheDate.Equals(date)).FirstOrDefaultAsync();

                if (dateType != null)
                {
                    holidayDimensions.Add(dateType);
                }
            }
            return holidayDimensions.ToList();

        }

        public async Task<ActionResult<List<TheCalendar>>> GetTypeDates(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.Now;

            List<TheCalendar> holidayDimensions = new List<TheCalendar>();
            if (startDate > endDate)
            {
                return holidayDimensions;
            }




            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dateType = await _context.TheCalendars.Where(c => date.Equals(c.TheDate)).FirstOrDefaultAsync();


                if (dateType != null)
                {
                    holidayDimensions.Add(dateType);
                }
            }

            return holidayDimensions.ToList();
        }
    }
}
