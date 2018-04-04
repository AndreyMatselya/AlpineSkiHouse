using MediatR;

namespace AlpineSkiHouse.Web.Events
{ 
    public class PassActivated : INotification
    {
        public int PassActivationId { get; set; }
    }
}
