using System;
using System.Collections.Generic;
using MediatR;

namespace AlpineSkiHouse.Web.Events
{
    public class PurchaseCompleted : INotification
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string TransactionId { get; set; }

        public decimal TotalCost { get; set; }

        public List<PassPurchased> Passes { get; set; }        
    }
}
