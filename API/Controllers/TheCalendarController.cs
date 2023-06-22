using API.DTOs.CalendarDto;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/thecalendar")]
    [ApiController]
    public class TheCalendarController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        private static TheCalendarService TheCalendarService { get; set; }
        public TheCalendarController(
            SwpProjectContext context, 
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            TheCalendarService = new TheCalendarService(context, mapper);
        }


        //[HttpGet]
        //public async Task<ActionResult<List<TheCalendar>>> GetCalendarByStartDateAndEndDate(DateTime startDate, DateTime endDate)
        //{
        //    var calendars = await TheCalendarService.GetTypeDates(startDate, endDate);

        //    return calendars;
        //}

        [HttpGet]
        public async Task<ActionResult<List<TheCalendar>>> GetHolidaysDateByStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            var calendar = await TheCalendarService.GetTypeDates(startDate, endDate);

            if(calendar == null)
            {
                return NotFound();
            }

            return calendar;
        }
    }
}
