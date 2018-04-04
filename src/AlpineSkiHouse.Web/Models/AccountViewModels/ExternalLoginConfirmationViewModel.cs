using System.ComponentModel.DataAnnotations;

namespace AlpineSkiHouse.Web.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(70)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(70)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

    }
}
