using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Context
{
    public class Seed
    {
        public static async Task SeedData(UserManager<AppUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                      new AppUser{DisplayName = "Maciek", UserName = "maciek", Email = "maciek@gmail.com"},
                      new AppUser{DisplayName = "Paweł", UserName = "pawel", Email = "pawel@gmail.com"},
                      new AppUser{DisplayName = "Tomek", UserName = "tomek", Email = "tomek@gmail.com"}
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }
    }
}
