namespace API.DTOs.SkillDTO
{
    public class StaffSkillDto
    {
        public int UniqueId {get; set;}
        public int StaffId { get; set; }

        public int SkillId { get; set; }

        public string? Level { get; set; }


    }
}