using Microsoft.EntityFrameworkCore;

namespace Vehicles.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
           
        }

        public virtual DbSet<VehicleDetails> VehicleDetails { get; set; }
        public virtual DbSet<WeightCategories> WeightCategories{ get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<VehicleDetails>()
                        .HasOne(x => x.ManufacturerDetails)
                        .WithMany(x => x.Vehicles)
                        .HasForeignKey(x => x.ManufacturerId).IsRequired();

            builder.Entity<VehicleDetails>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Vehicles) 
                .HasForeignKey(x => x.CategoryId).IsRequired();

            // Initial seeding

            builder.Entity<Manufacturers>().HasData(
                new Manufacturers() { Id = 1, Name = "Mazda" },
                new Manufacturers() { Id = 2, Name = "Mercedes"},
                new Manufacturers() { Id = 3, Name = "Honda"},
                new Manufacturers() { Id = 4, Name = "Ferrari"},
                new Manufacturers() { Id = 5, Name = "Toyota"});

            builder.Entity<WeightCategories>().HasData(
                new WeightCategories() { Id = 1, Name = "Light", MinWeight = 0, MaxWeight = 500, IconId = 1},
                new WeightCategories() { Id = 2, Name = "Medium", MinWeight = 500, MaxWeight = 2500, IconId = 2},
                new WeightCategories() { Id = 3, Name = "Heavy", MinWeight = 2500, MaxWeight = 100000, IconId = 3});

        }
    }
}
