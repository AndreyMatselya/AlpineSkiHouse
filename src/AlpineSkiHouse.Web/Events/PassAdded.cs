using System;
using MediatR;

namespace AlpineSkiHouse.Web.Events
{
    /// <summary>
    /// An event notification that occurs when a pass is added to a ski card
    /// </summary>
    public class PassAdded : INotification
    {
        public int CardId { get; set; }
        public int PassId { get; set; }
        public int PassTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
