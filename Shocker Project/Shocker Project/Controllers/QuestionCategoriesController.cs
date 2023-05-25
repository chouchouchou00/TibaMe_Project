using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;

namespace Shocker_Project.Controllers
{
    public class QuestionCategoriesController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public QuestionCategoriesController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        // GET: QuestionCategories
        public async Task<IActionResult> Index()
        {
              return _context.QuestionCategories != null ? 
                          View(await _context.QuestionCategories.ToListAsync()) :
                          Problem("Entity set 'db_a98a02_thm101team1001Context.QuestionCategories'  is null.");
        }

        // GET: QuestionCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.QuestionCategories == null)
            {
                return NotFound();
            }

            var questionCategories = await _context.QuestionCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (questionCategories == null)
            {
                return NotFound();
            }

            return View(questionCategories);
        }

        // GET: QuestionCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuestionCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] QuestionCategories questionCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionCategories);
        }

        // GET: QuestionCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.QuestionCategories == null)
            {
                return NotFound();
            }

            var questionCategories = await _context.QuestionCategories.FindAsync(id);
            if (questionCategories == null)
            {
                return NotFound();
            }
            return View(questionCategories);
        }

        // POST: QuestionCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] QuestionCategories questionCategories)
        {
            if (id != questionCategories.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionCategoriesExists(questionCategories.CategoryId))
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
            return View(questionCategories);
        }

        // GET: QuestionCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.QuestionCategories == null)
            {
                return NotFound();
            }

            var questionCategories = await _context.QuestionCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (questionCategories == null)
            {
                return NotFound();
            }

            return View(questionCategories);
        }

        // POST: QuestionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.QuestionCategories == null)
            {
                return Problem("Entity set 'db_a98a02_thm101team1001Context.QuestionCategories'  is null.");
            }
            var questionCategories = await _context.QuestionCategories.FindAsync(id);
            if (questionCategories != null)
            {
                _context.QuestionCategories.Remove(questionCategories);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionCategoriesExists(int id)
        {
          return (_context.QuestionCategories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
