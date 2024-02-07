using Microsoft.AspNetCore.Identity;
using PetService.Domain;
using PetService.Domain.ValueObjects;

namespace PetService.Infrastructure
{
    public class Seed
    {
        private const string DefaultPassword = "Pa$$w0rd";

        public static void SeedData(DataContext context)
        {
            SeedBreads(context);
            SeedMissingPets(context);
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
