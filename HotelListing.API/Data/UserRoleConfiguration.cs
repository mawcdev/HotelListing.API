using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data
{
    public class UserRoleConfiguration
    {
        public static async Task SeedIdentityAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApiRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    var newAdminRole = new ApiRole
                    {
                        Name = UserRoles.Admin,
                        Description = UserRoles.Admin
                    };
                    await roleManager.CreateAsync(newAdminRole);
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    var newUserRole = new ApiRole
                    {
                        Name = UserRoles.User,
                        Description = UserRoles.User
                    };
                    await roleManager.CreateAsync(newUserRole);
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

                string adminUserEmail = "admin@etickets.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApiUser()
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    var user = await userManager.CreateAsync(newAdminUser, "123Qwe?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }
    }
}
