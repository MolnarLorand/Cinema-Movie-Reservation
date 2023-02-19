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
    public class DirectorsController : Controller
    {
        private readonly CinemaContext _context;

        public DirectorsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
              return View(await _context.Directors.ToListAsync());
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Directors == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.DirectorID == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DirectorID,FirstName,LastName")] Director director)
        {
            if (ModelState.IsValid)
            {
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Directors == null)
            {
                return NotFound();
            }

            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return View(director);
        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DirectorID,FirstName,LastName")] Director director)
        {
            if (id != director.DirectorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.DirectorID))
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
            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Directors == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.DirectorID == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Directors == null)
            {
                return Problem("Entity set 'CinemaContext.Directors'  is null.");
            }
            var director = await _context.Directors.FindAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
          return _context.Directors.Any(e => e.DirectorID == id);
        }
    }
}
