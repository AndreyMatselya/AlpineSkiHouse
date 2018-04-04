﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Models;
using AlpineSkiHouse.Web.Models.SkiCardViewModels;

namespace AlpineSkiHouse.Web.ViewComponents
{
    public class SkiCardsViewComponent : ViewComponent
    {
        public UserManager<ApplicationUser> _userManager { get; set; }
        public SkiCardContext _skiCardContext { get; set; }
        public SkiCardsViewComponent(SkiCardContext skiCardContext, 
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _skiCardContext = skiCardContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _userManager.GetUserId(User as ClaimsPrincipal);
            var skiCardsViewModels = await _skiCardContext.SkiCards
                .Where(s => s.ApplicationUserId == userId)
                .Select(s => new SkiCardListViewModel
                {
                    Id = s.Id,
                    CardHolderName = s.CardHolderFirstName + " " + s.CardHolderLastName
                })
                .ToListAsync();
            return View(skiCardsViewModels);
        }
    }
}
