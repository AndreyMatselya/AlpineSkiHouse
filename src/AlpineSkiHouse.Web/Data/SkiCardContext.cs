using AlpineSkiHouse.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AlpineSkiHouse.Web.Data
{
    public class SkiCardContext : DbContext
    {
        public SkiCardContext(DbContextOptions<SkiCardContext> options) : base(options)
        {

        }

        public DbSet<SkiCard> SkiCards { get; set; }
    }
}
