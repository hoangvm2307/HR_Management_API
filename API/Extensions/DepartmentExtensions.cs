using API.DTOs;
using API.DTOs.DepartmentDTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class DepartmentExtensions
    {
        public static IQueryable<DepartmentDto> ProjectDepartmentToDepartmentDto(this IQueryable<Department> query)
        {
            return query
                    .Select(department => new DepartmentDto
                    {
                        DepartmentId = department.DepartmentId,
                        DepartmentName = department.DepartmentName,
                        // UserInfors = department.UserInfors.Select(userInfo => new DepartmentUserInforDto
                        // {
                        //     StaffId = userInfo.StaffId,
                        //     //UserId = userInfo.Id,
                        //     LastName = userInfo.LastName,
                        //     FirstName = userInfo.FirstName,
                        //     Dob = userInfo.Dob,
                        //     Phone = userInfo.Phone,
                        //     //Gender = userInfo.Gender,
                        //     Address = userInfo.Address,
                        //     Country = userInfo.Country,
                        //     CitizenId = userInfo.CitizenId,
                        //     DepartmentId = userInfo.DepartmentId,
                        //     //Position = userInfo.Position,
                        //     //HireDate = userInfo.HireDate,
                        //     BankAccount = userInfo.BankAccount,
                        //     BankAccountName = userInfo.BankAccountName,
                        //     Bank = userInfo.Bank,
                        //     WorkTimeByYear = userInfo.WorkTimeByYear,
                        //     AccountStatus = userInfo.AccountStatus
                        // })
                        // .ToList()
                    }).AsNoTracking();
        }
    }
}