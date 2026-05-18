using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("Email Address")]
        [Required(ErrorMessage ="Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        //This will compare the confirm password with the passwork and make sure both of it matches.
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
