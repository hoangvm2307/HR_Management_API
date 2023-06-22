using API.DTOs.PersonnelContractDTO;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class PersonnelContractService
    {
        private readonly SwpProjectContext _context;
        private readonly IMapper _mapper;
        private readonly TheCalendarService _theCalendarService;
        private readonly UserInfoService _userInfoService;

        public PersonnelContractService(
            SwpProjectContext context,
            IMapper mapper,
            TheCalendarService theCalendarService,
            UserInfoService userInfoService
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _theCalendarService = theCalendarService ?? throw new ArgumentNullException(nameof(theCalendarService));
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
        }

        public async Task<List<PersonnelContractDTO>> GetPersonnelContractsAsync()
        {
            var personnelContracts = await _context.PersonnelContracts
                                            .Include(c => c.ContractType)
                                            .Include(c => c.Allowances)
                                            .ThenInclude(c => c.AllowanceType)
                                            .Include(c => c.Staff)
                                            .ToListAsync();

            var finalPersonnelContracts = _mapper.Map<List<PersonnelContractDTO>>(personnelContracts);

            return finalPersonnelContracts;
        }
        public async Task<PersonnelContractDTO> CreatePersonnelContract(int staffId, PersonnelContractCreationDTO personnelContractCreationDTO)
        {

            var PersonnelContract = _mapper.Map<PersonnelContract>(personnelContractCreationDTO);
            var userInfor = await GetUserInfoContract(staffId);

            userInfor.PersonnelContracts.Add(PersonnelContract);

            var returnPersonnelContract = _mapper.Map<PersonnelContractDTO>(PersonnelContract);

            return returnPersonnelContract;
        }

        private async Task<UserInfor?> GetUserInfoContract(int staffId)
        {
            return await _context.UserInfors
                .Include(c => c.PersonnelContracts)
                .Where(c => c.StaffId == staffId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PersonnelContractDTO>> GetPersonnelContractById(int staffId)
        {
            var personnelContract = await _context.PersonnelContracts
                                        .Include(c => c.ContractType)
                                        .Include(c => c.Allowances)
                                        .ThenInclude(c => c.AllowanceType)
                                        .Include(c => c.Staff)
                                        .Where(c => c.StaffId == staffId)
                                        .ToListAsync();

            var finalPersonnelContracts = _mapper.Map<List<PersonnelContractDTO>>(personnelContract);

            return finalPersonnelContracts;
        }

        public async Task<bool> IsContractTypeValid(int contractTypeId)
        {
            var IsContractTypeValid = await _context.ContractTypes.AnyAsync(c => c.ContractTypeId == contractTypeId);

            return IsContractTypeValid;
        }

        public async Task<PersonnelContractDTO> GetValidPersonnelContractByStaffId(int staffId)
        {
            var validPersonnelContract = await _context.PersonnelContracts
                .Include(c => c.ContractType)
                .Include(c => c.Allowances)
                .ThenInclude(c => c.AllowanceType)
                .Where(c => c.StaffId == staffId)
                .FirstOrDefaultAsync();

            var returnValidPersonnelContract = _mapper.Map<PersonnelContractDTO>(validPersonnelContract);
                
            return returnValidPersonnelContract;
        }

        public async Task<bool> IsContractTimeValid(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                return false;
            }

            if ((end - start).TotalDays < 30)
            {
                return false;
            }

            if (start < DateTime.Today)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsValidContractExist(int staffId)
        {
            var IsValidContractExist = await _context.PersonnelContracts.AnyAsync(c => c.ContractStatus == true && c.StaffId == staffId);

            return IsValidContractExist;
        }
    }
}
