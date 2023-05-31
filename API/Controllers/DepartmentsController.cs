using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class DepartmentsController : BaseApiController
    {
        private readonly SwpProjectContext _context;
        public DepartmentsController(SwpProjectContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return departments;
        }
    }
}