using System.Collections.Generic;

namespace AlpineSkiHouse.Web.Models
{
    public class Resort
    {
        public Resort() 
        {
            Locations = new HashSet<Location>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Location> Locations { get; set; }
    }
}
