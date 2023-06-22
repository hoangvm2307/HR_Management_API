using API.DTOs.LogOtDTOs;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/ot-types")]
    [ApiController]
    public class LogOtTypeController : ControllerBase
    {
        private readonly LogOtTypeService _logOtTypeService;

        public LogOtTypeController(LogOtTypeService logOtTypeService)
        {
            _logOtTypeService = logOtTypeService ?? throw new ArgumentNullException(nameof(logOtTypeService));
        }

        [HttpGet]
        
        public async Task<List<OtTypeDTO>> GetLogOtTypesAsync()
        {
            return await _logOtTypeService.GetLogOtType();
        }
    }
}
