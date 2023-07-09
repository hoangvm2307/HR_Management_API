﻿using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class DepartmentService
    {
        private readonly SwpProjectContext _context;

        public DepartmentService(SwpProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> IsDepartmentExist(int departmentId)
        {
            return await _context.Departments.AnyAsync(c => c.DepartmentId == departmentId);
        }

    }
}
