using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;

namespace Cinema.Controllers
{
    public class CynemaMoviesController : Controller
    {
        private readonly CinemaContext _context;

        public CynemaMoviesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: CynemaMovies
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.CynemaMovies.Include(c => c.Cynema).Include(c => c.Movie);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: CynemaMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CynemaMovies == null)
            {
                return NotFound();
            }

            var cynemaMovie = await _context.CynemaMovies
                .Include(c => c.Cynema)
                .Include(c => c.Movie)
                .FirstOrDefaultAsync(m => m.MovieID == id);
            if (cynemaMovie == null)
            {
                return NotFound();
            }

            return View(cynemaMovie);
        }

        // GET: CynemaMovies/Create
        public IActionResult Create()
        {
            ViewData["CynemaID"] = new SelectList(_context.Cynemas, "ID", "CinemaName");
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID");
            return View();
        }

        // POST: CynemaMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CynemaID,MovieID")] CynemaMovie cynemaMovie)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(cynemaMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CynemaID"] = new SelectList(_context.Cynemas, "ID", "CinemaName", cynemaMovie.CynemaID);
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID", cynemaMovie.MovieID);
            return View(cynemaMovie);
        }

        // GET: CynemaMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CynemaMovies == null)
            {
                return NotFound();
            }

            var cynemaMovie = await _context.CynemaMovies.FindAsync(id);
            if (cynemaMovie == null)
            {
                return NotFound();
            }
            ViewData["CynemaID"] = new SelectList(_context.Cynemas, "ID", "CinemaName", cynemaMovie.CynemaID);
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID", cynemaMovie.MovieID);
            return View(cynemaMovie);
        }

        // POST: CynemaMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CynemaID,MovieID")] CynemaMovie cynemaMovie)
        {
            if (id != cynemaMovie.MovieID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(cynemaMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CynemaMovieExists(cynemaMovie.MovieID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CynemaID"] = new SelectList(_context.Cynemas, "ID", "CinemaName", cynemaMovie.CynemaID);
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID", cynemaMovie.MovieID);
            return View(cynemaMovie);
        }

        // GET: CynemaMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CynemaMovies == null)
            {
                return NotFound();
            }

            var cynemaMovie = await _context.CynemaMovies
                .Include(c => c.Cynema)
                .Include(c => c.Movie)
                .FirstOrDefaultAsync(m => m.MovieID == id);
            if (cynemaMovie == null)
            {
                return NotFound();
            }

            return View(cynemaMovie);
        }

        // POST: CynemaMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CynemaMovies == null)
            {
                return Problem("Entity set 'CinemaContext.CynemaMovies'  is null.");
            }
            var cynemaMovie = await _context.CynemaMovies.FindAsync(id);
            if (cynemaMovie != null)
            {
                _context.CynemaMovies.Remove(cynemaMovie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CynemaMovieExists(int id)
        {
          return _context.CynemaMovies.Any(e => e.MovieID == id);
        }
    }
}
