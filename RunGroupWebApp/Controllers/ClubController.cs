using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Club> clubs = _context.Clubs.ToList();
            //sends the data to the respective view.
            return View(clubs);
        }

        public IActionResult Detail(int Id)
        {
            Club club = _context.Clubs.Include(c => c.Address).FirstOrDefault(c => c.Id == Id);
            return View(club);
        }
    }
}
