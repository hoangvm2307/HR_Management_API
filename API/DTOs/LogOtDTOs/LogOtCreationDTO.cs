using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.LogOtDTOs
{
    public class LogOtCreationDTO
    {

        public string LogTitile { get; set; } = null!;

        public DateTime LogStart { get; set; }

        public DateTime LogEnd { get; set; }

        public double LogHours { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}