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
        private readonly TheCalendarService _theCalendarService;

        public TheCalendarController(
            SwpProjectContext context,
            IMapper mapper,
            TheCalendarService theCalendarService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _theCalendarService = theCalendarService ?? throw new ArgumentNullException(nameof(theCalendarService));
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
            var calendar = await _theCalendarService.GetTypeDates(startDate, endDate);

            if (calendar == null)
            {
                return NotFound();
            }

            return calendar;
        }

    }
}
