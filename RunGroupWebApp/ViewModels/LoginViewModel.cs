using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email address is required.")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
