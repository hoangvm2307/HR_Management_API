using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/date-dimension")]
    public class DateDimensionController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        public DateDimensionController(SwpProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            
        }

        [HttpGet]
        public async Task<ActionResult<List<DateDimension>>> GetDateDimensions()
        {
            var DateDimensions = await _context.DateDimensions.ToListAsync();

            return DateDimensions;
        }
    }
}