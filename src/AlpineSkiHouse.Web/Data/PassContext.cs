using AlpineSkiHouse.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AlpineSkiHouse.Web.Data
{
    public class PassContext : DbContext
    {
        public PassContext(DbContextOptions<PassContext> options) 
            :base(options)
        {
        }

        public DbSet<Pass> Passes { get; set; }

        public DbSet<PassActivation> PassActivations { get; set; }

        public DbSet<Scan> Scans { get; set; }
    }
}
