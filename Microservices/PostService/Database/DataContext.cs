using Microsoft.EntityFrameworkCore;
using PostService.Entities;

namespace PostService.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MissingPet> MissingPets { get; set; }
    }
}
