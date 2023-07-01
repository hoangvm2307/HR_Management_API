using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserInfoService
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;

        public UserInfoService
            (SwpProjectContext context, IMapper mapper, TheCalendarService theCalendarService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        
        public async Task<bool> IsUserExist(int StaffId)
        {
            return await _context.UserInfors.Where(c => c.StaffId == StaffId && c.AccountStatus == true).AnyAsync();
        }

        public async Task<bool> IsSaveChangeAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

    }
}
