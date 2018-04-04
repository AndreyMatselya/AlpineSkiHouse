using MediatR;

namespace AlpineSkiHouse.Web.Events
{
    /// <summary>
    /// An event notification that occurs when a scan has occurred
    /// </summary>
    public class CardScanned : INotification
    {
        public int ScanId { get; set; }
    }
}
