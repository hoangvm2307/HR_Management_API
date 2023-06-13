namespace API.DTOs.TicketDTO
{
    public class TicketDto
    {
        public int TicketId { get; set; }

        public int StaffId { get; set; }

        public string StaffName { get; set; }

        public int? TicketTypeId { get; set; }

        public string TicketTitle { get; set; } 

        public string? TicketFile { get; set; }

        public string TicketStatus { get; set; } 

        public string? ProcessNote { get; set; }

        public int? RespondencesId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ChangeStatusTime { get; set; }
        
    }
}