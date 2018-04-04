using Microsoft.AspNetCore.Mvc;

namespace AlpineSkiHouse.Web.Controllers
{
    public class ConditionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}