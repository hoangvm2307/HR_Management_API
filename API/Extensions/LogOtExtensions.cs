using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Extensions
{
    public class LogOtExtensions
    {
        private readonly SwpProjectContext _context;
        public LogOtExtensions(SwpProjectContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));      
        }

        
    }
}