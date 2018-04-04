﻿using System.Threading.Tasks;
using AlpineSkiHouse.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AlpineSkiHouse.Web.Security
{
    public class EditSkiCardAuthorizationHandler :
        AuthorizationHandler<EditSkiCardAuthorizationRequirement, SkiCard>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditSkiCardAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                                    EditSkiCardAuthorizationRequirement requirement, 
                                    SkiCard skiCard)
        {
            var userId = _userManager.GetUserId(context.User);
            if (skiCard.ApplicationUserId == userId)
            {
                context.Succeed(requirement);
            }            
            return Task.CompletedTask;
        }
    }
}
