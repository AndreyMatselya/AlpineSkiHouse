using System.Threading.Tasks;

namespace AlpineSkiHouse.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
