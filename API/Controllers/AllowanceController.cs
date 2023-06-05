using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.AllowanceDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/allowance")]
    public class AllowanceController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public AllowanceController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            
        }

        [HttpGet]
        public async Task<List<AllowanceDTO>> GetAllowancesAsync()
        {
            var allowances = await _context.Allowances.ToListAsync();

            var finalAllowances = _mapper.Map<List<AllowanceDTO>>(allowances);

            return finalAllowances;
        }
    }
}