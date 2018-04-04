using Microsoft.AspNetCore.Mvc;

namespace AlpineSkiHouse.Web.ViewComponents
{
    public class MetersSkiedViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
