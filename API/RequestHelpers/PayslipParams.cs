namespace API.RequestHelpers
{
    public class PayslipParams : PaginationParms
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }

        public string? Departments { get; set; }

    }
}
