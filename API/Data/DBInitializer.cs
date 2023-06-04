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
            


            context.SaveChanges();
        }
    }
}