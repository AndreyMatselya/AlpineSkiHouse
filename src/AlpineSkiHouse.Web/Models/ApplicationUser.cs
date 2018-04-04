using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AlpineSkiHouse.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(70)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(70)]
        public string LastName { get; set; }
    }
}
