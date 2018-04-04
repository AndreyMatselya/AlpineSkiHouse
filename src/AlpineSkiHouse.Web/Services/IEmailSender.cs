using System.Threading.Tasks;

namespace AlpineSkiHouse.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
