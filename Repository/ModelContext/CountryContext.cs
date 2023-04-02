using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelContext
{

    public class CountryContext : DbContext
    {
        public CountryContext(DbContextOptions options) : base(options) 
        { 
        }

        public DbSet<Country> CountryContexts { get; set; } = null!;
        public DbSet<Regions> RegionContext { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=model;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasKey(c => c.Id);
            modelBuilder.Entity<Regions>().HasKey(r => r.Id);

            modelBuilder.Entity<Regions>()
                .HasOne(r => r.Country)
                .WithMany(c => c.OwnedRegions)
                .HasForeignKey(r => r.CountryId);
        }

    }
}
