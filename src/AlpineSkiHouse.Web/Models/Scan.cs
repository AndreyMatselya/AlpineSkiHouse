using System;

namespace AlpineSkiHouse.Web.Models
{
    public class Scan
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public int LocationId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
