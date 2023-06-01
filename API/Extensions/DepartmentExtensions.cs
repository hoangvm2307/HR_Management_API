using API.DTOs;
using API.DTOs.DepartmentDTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class DepartmentExtensions
    {
        public static IQueryable<DepartmentDTO> ProjectDepartmentToDepartmentDTO(this IQueryable<Department> query)
        {
            return query
                    .Select(department => new DepartmentDTO
                    {
                        DepartmentID = department.DepartmentId,
                        DepartmentName = department.DepartmentName,
                        UserInfors = department.UserInfors.Select(userInfo => new UserInforDTO
                        {
                            StaffId = userInfo.StaffId,
                            UserId = userInfo.UserId,
                            LastName = userInfo.LastName,
                            FirstName = userInfo.FirstName,
                            Dob = userInfo.Dob,
                            Phone = userInfo.Phone,
                            Gender = userInfo.Gender,
                            Address = userInfo.Address,
                            Country = userInfo.Country,
                            CitizenId = userInfo.CitizenId,
                            DepartmentId = userInfo.DepartmentId,
                            Position = userInfo.Position,
                            HireDate = userInfo.HireDate,
                            BankAccount = userInfo.BankAccount,
                            BankAccountName = userInfo.BankAccountName,
                            Bank = userInfo.Bank,
                            LeaveDayLeft = userInfo.LeaveDayLeft,
                            WorkTimeByYear = userInfo.WorkTimeByYear,
                            AccountStatus = userInfo.AccountStatus
                        })
                        .ToList()
                    }).AsNoTracking();
        }
    }
}