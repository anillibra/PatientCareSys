using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Models
{
    /// public class CareSysContext : DbContext
    public class CareSysContext : IdentityDbContext<AppUser>
    {
        public CareSysContext(DbContextOptions<CareSysContext> options) : base(options)
        { }

        public DbSet<CareHome> CareHomes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Write Fluent API configurations here
            // CareHome configuration
            modelBuilder.Entity<CareHome>()
                .Property(col => col.Name)
                    .HasMaxLength(150)
                    .IsRequired();
        }


    }
}
