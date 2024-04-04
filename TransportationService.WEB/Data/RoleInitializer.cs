using Microsoft.AspNetCore.Identity;
using TransportationService.WEB.Data.Entities;

namespace TransportationService.WEB.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //try
            //{
            //    await userManager.DeleteAsync(await userManager.FindByEmailAsync("maksoncheus@gmail.com"));
            //}
            //finally { }
            string adminEmail = "alinaFedchenkooO@gmail.com";
            string password = "!@_Aa123456";
            if (await roleManager.FindByNameAsync("Administrator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            if (await roleManager.FindByNameAsync("Manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }
            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = "AlinaFedchenko",
                    EmailConfirmed = true,
                    PhoneNumber = "88005553535",
                    FirstName = "Алина",
                    Patronymic = "Батьковна",
                    LastName = "Федченко"
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
