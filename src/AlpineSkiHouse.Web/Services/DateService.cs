using System;

namespace AlpineSkiHouse.Web.Services
{
    public class DateService : IDateService
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }

        public DateTime Today()
        {
            return DateTime.UtcNow.Subtract(DateTime.UtcNow.TimeOfDay);
        }
    }
}
