using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Seller.Controllers
{
    [Area("seller")]
    public class ProductsController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;
        public ProductsController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getProducts()
        {
            var product = from p in _context.Products
                          //where p.SellerAccount == User.Identity.Name
                          select new
                          {
                              ProductId = p.ProductId,
                              SellerAccount = p.SellerAccount,
                              LaunchDate = p.LaunchDate,
                              ProductName = p.ProductName,
                              ProductCategoryId = p.ProductCategoryId,
                              Description = p.Description,
                              UnitsInStock = p.UnitsInStock,
                              Sales = p.Sales,
                              UnitPrice = p.UnitPrice,
                              Status = p.Status,
                              Currency = p.Currency
                          };
            return Json(product);
        }
        [HttpGet]
        public async Task<JsonResult> Filter([Bind("ProductId, SellerAccount, LaunchDate, ProductName, ProductCategoriyID, Description, UnitsInStock, UnitPrice, Status, Currency")] Products product)
        {
            var query = _context.Products.Where(
                p => /*p.SellerAccount == User.Identity.Name ||*/
                     p.ProductName.Contains(product.ProductName) ||                     
                     p.Description.Contains(product.Description) ||
                     p.UnitsInStock.Contains(product.UnitsInStock) ||
                     p.UnitPrice.Contains(product.UnitPrice) ||
                     p.Status.Contains(product.Status) ||
                     p.Currency.Contains(product.Currency))
                     .Select(p => new {
                         ProductId = p.ProductId,
                         SellerAccount = p.SellerAccount,
                         LaunchDate = p.LaunchDate,
                         ProductName = p.ProductName,
                         ProductCategoryId = p.ProductCategoryId,
                         Description = p.Description,
                         UnitsInStock = p.UnitsInStock,
                         Sales = p.Sales,
                         UnitPrice = p.UnitPrice,
                         Status = p.Status,
                         Currency = p.Currency
                     });
            return Json(query);
        }
        [HttpGet]
        public async Task<JsonResult> Create([Bind("ProductName, ProductCategoriyID, Description, UnitsInStock, UnitPrice, Status, Currency")] Products product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Json(new { Results = "OK", Record = product });
            }
            else
            {
                return Json(new { Results = "Error", Message = "新增失敗" });
            }
        }
        [HttpGet]
        public async Task<JsonResult> Update([Bind("ProductName, ProductCategoriyID, Description, UnitsInStock, UnitPrice, Status, Currency")] Products product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Products p = await _context.Products.FindAsync(product.ProductId);
                    p.ProductName = product.ProductName;
                    p.Description = product.Description;
                    _context.Update(p);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return Json(new { Result = "Error", Message = "記錄不存在" });
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(product);
            }
            else
            {
                return Json(new { Result = "Error", Message = "修改記錄失敗" });
            }
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(p => p.ProductId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<JsonResult> Delete(int id)
        {
            Products product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return Json(new { Result = "Error", Message = "找不到欲刪除記錄" });
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Json(product);
        }
        [HttpGet]
        public async Task<JsonResult> GetPicture(int id)
        {
            var picture = from p in _context.Pictures
                          where p.ProductId == id
                          select new
                          {
                              Path = p.Path,
                              Description = p.Description
                          };
            return Json(picture);
        }
    }
}
