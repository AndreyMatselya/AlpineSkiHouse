namespace AlpineSkiHouse.Web.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Altitude { get; set; }

        public int ResortId { get; set; }
    }
}
