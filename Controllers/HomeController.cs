using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models.LibraryViewModels;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly CinemaContext _context;
        public HomeController(CinemaContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Statistics()
        {
            IQueryable<ReservationGroup> data =
            from reservation in _context.Reservations
            group reservation by reservation.ReservationDate into dateGroup
            select new ReservationGroup()
            {
                ReservedDate = dateGroup.Key,
                MovieCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}