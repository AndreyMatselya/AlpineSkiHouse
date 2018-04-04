namespace AlpineSkiHouse.Web.Models
{
    public class PassActivation
    {
        public int Id { get; set; }

        public int PassId { get; set; }

        public int ScanId { get; set; }

        public Scan Scan { get; set; }
    }
}
