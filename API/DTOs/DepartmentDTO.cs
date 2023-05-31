namespace API.DTOs.DepartmentDTO
{
    public class DepartmentDTO
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public List<UserInforDTO> UserInfors {get;set;}
    }
}