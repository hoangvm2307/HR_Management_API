namespace API.DTOs.UserInforDTO
{
    public class LogOtDto
    {
        public int OtLogId { get; set; }

        public int StaffId { get; set; }

        public string LogTitile { get; set; } = null!;

        public DateTime LogStart { get; set; }

        public DateTime LogEnd { get; set; }

        public double LogHours { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}