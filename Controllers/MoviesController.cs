using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;
using Microsoft.Data.SqlClient;
using static System.Reflection.Metadata.BlobBuilder;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaContext _context;

        public MoviesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var movies = from b in _context.Movie
                        join a in _context.Directors on b.DirectorID equals a.DirectorID
                        select new Movie
                        {
                            ID = b.ID,
                            Title = b.Title,
                            Genre = b.Genre,
                            Price = b.Price,
                            Director = a
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    movies = movies.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    movies = movies.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    movies = movies.OrderByDescending(b => b.Price);
                    break;
                default:
                    movies = movies.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Movie>.CreateAsync(movies.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.
                Include(m => m.Director).
                Include(s => s.Reservations).
                ThenInclude(e => e.Customer).
                AsNoTracking().
                FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["LastName"] = new SelectList(_context.Directors, "DirectorID", "LastName");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Genre,Price,DirectorID")] Movie movie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }

            /*            ViewData["DirectorID"] = new SelectList(_context.Directors, "DirectorID", "DirectorID", movie.DirectorID);
            */
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["LastName"] = new SelectList(_context.Directors, "DirectorID", "LastName", movie.DirectorID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movieToUpdate = await _context.Movie.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Movie>(movieToUpdate, "",s => s.DirectorID, s => s.Title, s => s.Price, s => s.Genre))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(movieToUpdate);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .AsNoTracking()
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }


            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        private bool MovieExists(int id)
        {
          return _context.Movie.Any(e => e.ID == id);
        }
    }
}
