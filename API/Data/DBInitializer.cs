using System.Diagnostics;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(SwpProjectContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "bob",
                    Email = "bob@test.com"
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Staff");

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] {"Staff", "HRStaff", "HRManager"});
            }
            
            if(!context.Departments.Any())
            {

                var departments = new List<Department>()
                {
                    new Department{ DepartmentName = "Sales"},
                    new Department{ DepartmentName = "Marketing"},
                    new Department{ DepartmentName = "Finance"},
                    new Department{ DepartmentName = "Human Resource"},
                    new Department{ DepartmentName = "Operations"},
                    new Department{ DepartmentName = "Engineering"},
                    new Department{ DepartmentName = "Customer Support"},
                    new Department{ DepartmentName = "Research & Development"},
                    new Department{ DepartmentName = "Quality Assurance"},
                    new Department{ DepartmentName = "Design"},
                };

                foreach(var department in departments)
                {
                    context.Departments.Add(department);
                }
            }


            context.SaveChanges();
        }
    }
}