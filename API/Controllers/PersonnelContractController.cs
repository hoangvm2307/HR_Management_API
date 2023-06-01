
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PersonnelContractController : BaseApiController
    { 
        private readonly SwpProjectContext _context;
        public PersonnelContractController(SwpProjectContext context)
        {
            _context = context;
        }

    }
}