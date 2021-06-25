using Microsoft.EntityFrameworkCore;
using Inventory.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Inventory.Api.DbContexts
{
    public class InventoryDbContext : DbContext
    {

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
                : base(options)
        {

        }

        public DbSet<TruckInventoryDto> TruckInventory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Truck>()
            //    .Property(t => t.TruckId)
            //    .HasDefaultValueSql("gen_random_uuid()");
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.Name)
            //     .IsRequired();
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.Description)
            //     .IsRequired();
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.Damaged)
            //     .HasDefaultValue(false)
            //     .ValueGeneratedOnAdd();
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.Hidden)
            //     .HasDefaultValue(false)
            //     .ValueGeneratedOnAdd();
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.Quantity)
            //     .HasDefaultValue(1)
            //     .ValueGeneratedOnAdd();
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.Price)
            //     .HasPrecision(18, 2);
            // modelBuilder.Entity<Truck>()
            //     .Property(t => t.PreviousPrice)
            //     .HasPrecision(18, 2);


            // modelBuilder.Entity<Category>()
            //    .Property(t => t.IsMiniTruck)
            //    .HasDefaultValue(false);
            // // .ValueGeneratedOnAdd();

            // modelBuilder.Entity<Photo>()
            //  .Property(t => t.PhotoId)
            //  .HasDefaultValueSql("gen_random_uuid()");

            base.OnModelCreating(modelBuilder);
        }
    }
}