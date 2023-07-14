namespace API.Services
{
    public class PayslipInputCreationDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int CreatorId { get; set; }
        public int? ChangerId { get; set; }
    }
}
