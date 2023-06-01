using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/contract")]
    public class ContractController : ControllerBase
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public ContractController(SwpProjectContext context, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonnelContractDTO>>> GetPersonnelContracts()
        {
            var PersonnelContracts = await _context.PersonnelContracts  
                                            .Include(c => c.ContractType)
                                            .Include(c => c.Allowances)
                                            .ThenInclude(c => c.AllowanceType)
                                            .ToListAsync();

            var finalPersonnelContracts = _mapper.Map<List<PersonnelContractDTO>>(PersonnelContracts);

            return finalPersonnelContracts;
        }

        [HttpGet("staffId")]
        public async Task<ActionResult<PersonnelContract>> GetPersonelContractByStaffId(int staffId)
        {
            var PersonnelContract = await _context.PersonnelContracts
                                        .Include(c => c.Allowances)
                                        .Where(c => c.StaffId == staffId)
                                        .FirstOrDefaultAsync();

            if(PersonnelContract == null)
            {
                return NotFound();
            }

            return PersonnelContract;
        }
    }
}