using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Shoppingcart.Controllers
{
    [Area("Shoppingcart")]
    public class ShoppingCartProductsController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public ShoppingCartProductsController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        // GET: Shoppingcart/ShoppingCartProducts
        public async Task<IActionResult> Index()
        {
            var db_a98a02_thm101team1001Context = _context.Products.Include(p => p.ProductCategory).Include(p => p.SellerAccountNavigation);
            return View(await db_a98a02_thm101team1001Context.ToListAsync());
        }

        // GET: Shoppingcart/ShoppingCartProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.SellerAccountNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Shoppingcart/ShoppingCartProducts/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName");
            ViewData["SellerAccount"] = new SelectList(_context.Users, "Account", "Account");
            return View();
        }

        // POST: Shoppingcart/ShoppingCartProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SellerAccount,LaunchDate,ProductName,ProductCategoryId,Description,UnitsInStock,Sales,UnitPrice,Status,Currency")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", products.ProductCategoryId);
            ViewData["SellerAccount"] = new SelectList(_context.Users, "Account", "Account", products.SellerAccount);
            return View(products);
        }

        // GET: Shoppingcart/ShoppingCartProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", products.ProductCategoryId);
            ViewData["SellerAccount"] = new SelectList(_context.Users, "Account", "Account", products.SellerAccount);
            return View(products);
        }

        // POST: Shoppingcart/ShoppingCartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,SellerAccount,LaunchDate,ProductName,ProductCategoryId,Description,UnitsInStock,Sales,UnitPrice,Status,Currency")] Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
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
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", products.ProductCategoryId);
            ViewData["SellerAccount"] = new SelectList(_context.Users, "Account", "Account", products.SellerAccount);
            return View(products);
        }

        // GET: Shoppingcart/ShoppingCartProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.SellerAccountNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Shoppingcart/ShoppingCartProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'db_a98a02_thm101team1001Context.Products'  is null.");
            }
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
