using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public IFormFile Image { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
    }
}
