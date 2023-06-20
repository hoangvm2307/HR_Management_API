using API.Entities;

namespace API.DTOs.LeaveDayDetailDTO
{
    public class LeaveDayDetailCreationDTO
    {
        //public int LeaveDayDetailId { get; set; }

        //public int? StaffId { get; set; }

        public int? LeaveTypeId { get; set; }

        public int? DayLeft { get; set; }

        public int? Time { get; set; }

        //public virtual LeaveDayDetailTypeDTO? LeaveType { get; set; }

        //public virtual UserInfor? Staff { get; set; }
    }
}
