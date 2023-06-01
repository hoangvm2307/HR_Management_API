using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PayslipDTOs
{
    public class PayslipDTO
    {
        public int PayslipId { get; set; }
        public int StaffId { get; set; }
        public int ContractId { get; set; }
        public float StandardWorkDays { get; set; }
        public float ActualWorkDays { get; set; }
        public float OtHours { get; set; }
        public float LeaveDays { get; set; }
        public int GrossSalary { get; set; }
        public int BHXHEmp { get; set; }
        public int BHYTEmp { get; set; }
        public int BHTNEmp { get; set; }
        //Giam tru gia canh ca nhan
        public int SelfAllowances { get; set; }
        //Giam tru gia canh nguoi phu thuoc
        public int NoOfDependences { get; set; }
        public int FamilyAllowances { get; set; }
        //Thu nhap truoc thue
        public int SalaryBeforeTax { get; set; }

        public int TaxRate5M { get; set; }

        public int TaxRate5MTo10M { get; set; }
        public int TaxRate10MTo18M { get; set; }
        public int TaxRate18MTo23M { get; set; }
        public int TaxRate23MTo52M { get; set; }
        public int TaxRate52MTo82M { get; set; }
        public int TaxRateOver82M { get; set; }

        public int Bonus { get; set; }
        //Deduction Trong truong hop nay la gi
        public int Deducion { get; set; }
        public int NetSalary { get; set; }
        // Paid By Date, Paid Date, CreateAt khac nhau cho nao
        public int PaidByDate { get; set; }
        public DateTime PaidDate { get; set; }

        ///--------------Cong ty tra -----------------
        public int BHXHComp { get; set; }
        public int BHYTComp { get; set; }
        public int BHTNComp { get; set; }
        //tự thêm ngọc chưa thêm
        public int TotalCompanyPaid { get; set; }
        //----------------------------------------------
        public DateTime CreateAt { get; set; }
        public bool PayslipStatus { get; set; }
    }
}