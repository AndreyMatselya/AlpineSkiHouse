using MediatR;

namespace AlpineSkiHouse.Web.Events
{
    public class SkiCardImageUploaded : INotification
    {
        public string FileName { get; set; }
    }
}
