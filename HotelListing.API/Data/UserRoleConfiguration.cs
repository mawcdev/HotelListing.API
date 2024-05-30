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
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }

                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
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
