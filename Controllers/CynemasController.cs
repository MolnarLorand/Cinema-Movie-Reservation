using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;
using Cinema.Models.LibraryViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Controllers
{
    public class CynemasController : Controller
    {
        private readonly CinemaContext _context;

        public CynemasController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Cynemas
        public async Task<IActionResult> Index(int? id, int? movieID)
        {
            var viewModel = new CynemaIndexData();
            viewModel.Cynemas = await _context.Cynemas
            .Include(i => i.CynemaMovies)
            .ThenInclude(i => i.Movie)
            .ThenInclude(i => i.Reservations)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.CinemaName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["CynemaID"] = id.Value;
                Cynema cynema = viewModel.Cynemas.Where(
                i => i.ID == id.Value).Single();
                viewModel.Movies = cynema.CynemaMovies.Select(s => s.Movie);
            }
            if (movieID != null)
            {
                ViewData["MovieID"] = movieID.Value;
                viewModel.Reservations = viewModel.Movies.Where(
                x => x.ID == movieID).Single().Reservations;
            }
            return View(viewModel);
        }


        // GET: Cynemas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cynemas == null)
            {
                return NotFound();
            }

            var cynema = await _context.Cynemas
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cynema == null)
            {
                return NotFound();
            }

            return View(cynema);
        }

        // GET: Cynemas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cynemas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CinemaName,Adress")] Cynema cynema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cynema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cynema);
        }

        // GET: Cynemas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cynema = await _context.Cynemas
            .Include(i => i.CynemaMovies).ThenInclude(i => i.Movie)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (cynema == null)
            {
                return NotFound();
            }
            PopulateCynemaMovieData(cynema);
            return View(cynema);

        }
        private void PopulateCynemaMovieData(Cynema cynema)
        {
            var allMovies = _context.Movie;
            var cynemaMovies = new HashSet<int>(cynema.CynemaMovies.Select(c => c.MovieID));
            var viewModel = new List<CynemaMovieData>(); foreach (var movie in allMovies)
            {
                viewModel.Add(new CynemaMovieData
                {
                    MovieID = movie.ID,
                    Title = movie.Title,
                    IsPublished = cynemaMovies.Contains(movie.ID)
                });
            }
            ViewData["Movies"] = viewModel;
        }


        // POST: Cynemas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedMovie)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cynemaToUpdate = await _context.Cynemas
            .Include(i => i.CynemaMovies)
            .ThenInclude(i => i.Movie)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Cynema>(
            cynemaToUpdate,
            "",
            i => i.CinemaName, i => i.Adress))
            {
                UpdateCynemaMovie(selectedMovie, cynemaToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateCynemaMovie(selectedMovie, cynemaToUpdate);
            PopulateCynemaMovieData(cynemaToUpdate);
            return View(cynemaToUpdate);
        }
        private void UpdateCynemaMovie(string[] selectedMovies, Cynema cynemaToUpdate)
        {
            if (selectedMovies == null)
            {
                cynemaToUpdate.CynemaMovies = new List<CynemaMovie>();
                return;
            }
            var selectedMoviesHS = new HashSet<string>(selectedMovies);
            var cynemaMovies = new HashSet<int>
            (cynemaToUpdate.CynemaMovies.Select(c => c.Movie.ID));
            foreach (var play in _context.Movie)
            {
                if (selectedMoviesHS.Contains(play.ID.ToString()))
                {
                    if (!cynemaMovies.Contains(play.ID))
                    {
                        cynemaToUpdate.CynemaMovies.Add(new CynemaMovie
                        {
                            CynemaID =
                       cynemaToUpdate.ID,
                            MovieID = play.ID
                        });
                    }
                }
                else
                {
                    if (cynemaMovies.Contains(play.ID))
                    {
                        CynemaMovie playToRemove = cynemaToUpdate.CynemaMovies.FirstOrDefault(i
                       => i.MovieID == play.ID);
                        _context.Remove(playToRemove);
                    }
                }
            }
        }

        // GET: Cynemas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cynemas == null)
            {
                return NotFound();
            }

            var cynema = await _context.Cynemas
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cynema == null)
            {
                return NotFound();
            }

            return View(cynema);
        }

        // POST: Cynemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cynemas == null)
            {
                return Problem("Entity set 'CinemaContext.Cynemas'  is null.");
            }
            var cynema = await _context.Cynemas.FindAsync(id);
            if (cynema != null)
            {
                _context.Cynemas.Remove(cynema);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CynemaExists(int id)
        {
          return _context.Cynemas.Any(e => e.ID == id);
        }
    }
}
