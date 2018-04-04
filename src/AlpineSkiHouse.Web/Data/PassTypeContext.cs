using AlpineSkiHouse.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AlpineSkiHouse.Web.Data
{
    public class PassTypeContext : DbContext
    {
        public PassTypeContext(DbContextOptions<PassTypeContext> options)
            :base(options)
        {
        }

        public DbSet<PassType> PassTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PassTypeResort>()
                .HasKey(p => new { p.PassTypeId, p.ResortId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
