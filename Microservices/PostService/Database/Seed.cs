using PostService.Entities;

namespace PostService.Database
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            SeedMissingPets(context);
        }

        private static void SeedMissingPets(DataContext context)
        {
            if (!context.MissingPets.Any())
            {
                var missingPets = new List<MissingPet>
                {
                    new MissingPet {Id = Guid.NewGuid(), Name = "Tara", Description = "Mała słodka",  MissingDate = DateTime.Now},
                    new MissingPet {Id = Guid.NewGuid(), Name = "Sara", Description = "Mała słodka",  MissingDate = DateTime.Now}
                };

                context.MissingPets.AddRange(missingPets);
                context.SaveChanges();
            }
        }
    }
}
