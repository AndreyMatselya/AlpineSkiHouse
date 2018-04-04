using AlpineSkiHouse.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AlpineSkiHouse.Web.Data
{
    public class ResortContext : DbContext
    {
        public ResortContext(DbContextOptions<ResortContext> options)
            :base(options)
        {
        }

        public DbSet<Resort> Resorts { get; set; }

        public DbSet<Location> Locations { get; set; }
    }
}
