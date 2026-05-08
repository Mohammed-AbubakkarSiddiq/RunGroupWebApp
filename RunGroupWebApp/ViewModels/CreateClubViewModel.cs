using RunGroupWebApp.Data.Enums;
using RunGroupWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public Address Address { get; set; }
        /// <summary>
        /// Gets or sets the category of the club.
        /// Stored as an integer in the database representing the <see cref="ClubCategory"/> enum int value.
        /// </summary>
        public ClubCategory ClubCategory { get; set; }
    }
}
