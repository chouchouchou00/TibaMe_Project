﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerReplyController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public CustomerReplyController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        // GET: Admin/CustomerReply
        public async Task<IActionResult> Index()
        {
            var db_a98a02_thm101team1001Context = _context.ClientCases.Include(c => c.AdminAccountNavigation).Include(c => c.QuestionCategory).Include(c => c.UserAccountNavigation);
            return View(await db_a98a02_thm101team1001Context.ToListAsync());
        }

        // GET: Admin/CustomerReply/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClientCases == null)
            {
                return NotFound();
            }

            var clientCases = await _context.ClientCases
                .Include(c => c.AdminAccountNavigation)
                .Include(c => c.QuestionCategory)
                .Include(c => c.UserAccountNavigation)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (clientCases == null)
            {
                return NotFound();
            }

            return View(clientCases);
        }

        // GET: Admin/CustomerReply/Create
        public IActionResult Create()
        {
            ViewData["AdminAccount"] = new SelectList(_context.Users, "Account", "Account");
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategories, "CategoryId", "CategoryName");
            ViewData["UserAccount"] = new SelectList(_context.Users, "Account", "Account");
            return View();
        }

        // POST: Admin/CustomerReply/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseId,QuestionCategoryId,UserAccount,AdminAccount,Description,Status,CloseDate,Reply")] ClientCases clientCases)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientCases);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminAccount"] = new SelectList(_context.Users, "Account", "Account", clientCases.AdminAccount);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategories, "CategoryId", "CategoryName", clientCases.QuestionCategoryId);
            ViewData["UserAccount"] = new SelectList(_context.Users, "Account", "Account", clientCases.UserAccount);
            return View(clientCases);
        }

        // GET: Admin/CustomerReply/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClientCases == null)
            {
                return NotFound();
            }

            var clientCases = await _context.ClientCases.FindAsync(id);
            if (clientCases == null)
            {
                return NotFound();
            }
            ViewData["AdminAccount"] = new SelectList(_context.Users, "Account", "Account", clientCases.AdminAccount);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategories, "CategoryId", "CategoryName", clientCases.QuestionCategoryId);
            ViewData["UserAccount"] = new SelectList(_context.Users, "Account", "Account", clientCases.UserAccount);
            return View(clientCases);
        }

        // POST: Admin/CustomerReply/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseId,QuestionCategoryId,UserAccount,AdminAccount,Description,Status,CloseDate,Reply")] ClientCases clientCases)
        {
            if (id != clientCases.CaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientCases);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientCasesExists(clientCases.CaseId))
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
            ViewData["AdminAccount"] = new SelectList(_context.Users, "Account", "Account", clientCases.AdminAccount);
            ViewData["QuestionCategoryId"] = new SelectList(_context.QuestionCategories, "CategoryId", "CategoryName", clientCases.QuestionCategoryId);
            ViewData["UserAccount"] = new SelectList(_context.Users, "Account", "Account", clientCases.UserAccount);
            return View(clientCases);
        }

        // GET: Admin/CustomerReply/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClientCases == null)
            {
                return NotFound();
            }

            var clientCases = await _context.ClientCases
                .Include(c => c.AdminAccountNavigation)
                .Include(c => c.QuestionCategory)
                .Include(c => c.UserAccountNavigation)
                .FirstOrDefaultAsync(m => m.CaseId == id);
            if (clientCases == null)
            {
                return NotFound();
            }

            return View(clientCases);
        }

        // POST: Admin/CustomerReply/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClientCases == null)
            {
                return Problem("Entity set 'db_a98a02_thm101team1001Context.ClientCases'  is null.");
            }
            var clientCases = await _context.ClientCases.FindAsync(id);
            if (clientCases != null)
            {
                _context.ClientCases.Remove(clientCases);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientCasesExists(int id)
        {
          return (_context.ClientCases?.Any(e => e.CaseId == id)).GetValueOrDefault();
        }
    }
}