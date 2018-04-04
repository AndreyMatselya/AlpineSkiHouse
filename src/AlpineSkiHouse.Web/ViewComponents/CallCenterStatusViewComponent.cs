using AlpineSkiHouse.Web.Models.CallCenterViewModels;
using AlpineSkiHouse.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlpineSkiHouse.Web.ViewComponents
{
    public class CallCenterStatusViewComponent : ViewComponent
    {
        private readonly ICsrInformationService _csrInformationService;

        public CallCenterStatusViewComponent(ICsrInformationService csrInformationService)
        {
            _csrInformationService = csrInformationService;
        }

        public IViewComponentResult Invoke()
        {
            if (_csrInformationService.CallCenterOnline)
            {
                var viewModel = new CallCenterStatusViewModel
                {
                    OnlineRepresentatives = _csrInformationService.OnlineRepresentatives,
                    PhoneNumber = _csrInformationService.CallCenterPhoneNumber
                };
                return View(viewModel);
            }
            else
            {
                return View("Closed");
            }
        }
    }
}
