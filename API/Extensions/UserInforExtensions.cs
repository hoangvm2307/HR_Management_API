using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.UserInforDTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserInforExtensions
    {
        public static IQueryable<UserInforDto> ProjectUserInforToUserInforDto(this IQueryable<UserInfor> query)
        {
            return query.Select(userInfor => new UserInforDto
            {
                //StaffId = userInfor.StaffId,
                //UserId = userInfor.Id,
                LastName = userInfor.LastName,
                FirstName = userInfor.FirstName,
                Dob = userInfor.Dob,
                Phone = userInfor.Phone,
                Gender = userInfor.Gender,
                Address = userInfor.Address,
                Country = userInfor.Country,
                CitizenId = userInfor.CitizenId,
                DepartmentId = userInfor.DepartmentId,
                Position = userInfor.Position,
                HireDate = userInfor.HireDate,
                BankAccount = userInfor.BankAccount,
                BankAccountName = userInfor.BankAccountName,
                Bank = userInfor.Bank,
                WorkTimeByYear = userInfor.WorkTimeByYear,
                AccountStatus = userInfor.AccountStatus
            }).AsNoTracking();
        }
    }
}