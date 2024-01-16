using Microsoft.AspNetCore.Identity;
using PetService.Domain;
using PetService.Domain.ValueObjects;

namespace PetService.Infrastructure
{
    public class Seed
    {
        private const string DefaultPassword = "Pa$$w0rd";

        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            await SeedUsers(userManager);
            SeedBreads(context);
            SeedMissingPets(context);
        }

        private static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser {DisplayName = "Maciek", UserName = "maciek", Email = "maciek@gmail.com", Bio = "a"},
                    new AppUser {DisplayName = "Paweł", UserName = "pawel", Email = "pawel@gmail.com", Bio = "a"},
                    new AppUser {DisplayName = "Tomek", UserName = "tomek", Email = "tomek@gmail.com", Bio = "a"}
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, DefaultPassword);
                }
            }
        }

        private static void SeedBreads(DataContext context)
        {
            if (!context.Breads.Any())
            {
                var breads = new List<Bread>
                {
                    new Bread(){Id = Guid.NewGuid(), Type = "labrador"},
                    new Bread(){Id = Guid.NewGuid(), Type = "mieszaniec"},
                    new Bread(){Id = Guid.NewGuid(), Type = "unknown"}

                };

                context.Breads.AddRange(breads);
                context.SaveChanges();
            }
        }

        private static void SeedMissingPets(DataContext context)
        {
            if (!context.MissingPets.Any())
            {

                var b1 = context.Breads.FirstOrDefault(b => b.Type == "labrador");
                var b2 = context.Breads.FirstOrDefault(b => b.Type == "mieszaniec");

                // if (b1 == null || b2 == null) return;

                var missingPets = new List<MissingPet>
                {
                    new MissingPet {Id = Guid.NewGuid(), Name = "Tara", Description = "Mała słodka", BreadId = b1.Id, MissingDate = DateTime.Now},
                    new MissingPet {Id = Guid.NewGuid(), Name = "Sara", Description = "Mała słodka", BreadId = b2.Id, MissingDate = DateTime.Now}
                };

                missingPets.ForEach(m => m.AddPhoto("link/to/photo"));

                context.MissingPets.AddRange(missingPets);
                context.SaveChanges();
            }
        }
    }
}
