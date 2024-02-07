using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetService.Domain;
using PetService.Domain.ValueObjects;

namespace PetService.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MissingPet> MissingPets { get; set; }
        public DbSet<Bread> Breads { get; set; }
        public DbSet<MissingPetPhoto> MissingPetPhotos { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     modelBuilder.Entity<MissingPet>()
        //       .HasOne(mp => mp.Bread)
        //       .WithOne()
        //       .HasForeignKey<Bread>(b => b.Id);

        //     modelBuilder.Entity<Bread>()
        //     .Property(e => e.Id)
        //     .HasConversion(
        //         v => v.ToString(),   // Convert GUID to string when storing in the database
        //         v => new Guid(v));    // Convert string to GUID when reading from the database

        //     // One-to-many relationship between MissingPet and MissingPetPhoto
        //     modelBuilder.Entity<MissingPetPhoto>()
        //         .HasOne(pp => pp.MissingPet)
        //         .WithMany(mp => mp.Photos)
        //         .HasForeignKey(pp => pp.MissingPetId);
        // }
    }
}
