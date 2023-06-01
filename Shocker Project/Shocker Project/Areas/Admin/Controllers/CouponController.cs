using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Areas.Admin.Models;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;
        public CouponController(db_a98a02_thm101team1001Context context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> DisaplayCoupon() 
        {
            var DC = _context.Coupons
                .Select(x => new
                {
                    CouponId = x.CouponId,
                    ExpirationDate = x.ExpirationDate,
                    HolderAccount = x.HolderAccount,
                    ProductCategoryId = x.ProductCategoryId,
                    Discount = x.Discount,
                    Status = x.Status,
                    PublisherAccount = x.PublisherAccount,
                });
            return Json(DC);
        }
        [HttpPost]
        public async Task<JsonResult> CreateCoupon([FromBody] CouponsViewModels cvm)
        {
            if (ModelState.IsValid)
            {
                //cvm.PublisherAccount = "Admin1";
                Coupons coupons = new Coupons()
                {
                    ExpirationDate = cvm.ExpirationDate,
                    HolderAccount = cvm.HolderAccount,
                    ProductCategoryId = cvm.ProductCategoryId,
                    Discount = cvm.Discount,
                    Status = cvm.Status,
                    PublisherAccount = cvm.PublisherAccount,
                    
                };

                _context.Coupons.Add(coupons);
                _context.SaveChanges();

            }
            return Json(new { Message = "建置完畢" });
        }
        [HttpPost]
        public JsonResult FilterCoupon([FromBody]CouponsViewModels cvm) 
        {
            
            return Json(_context.Coupons.Where(x =>
                      x.HolderAccount.Contains(cvm.HolderAccount) ||
                      x.Discount == cvm.Discount ||
                      x.StatusNavigation.StatusName.Contains(cvm.StatusName) ||
                      x.ProductCategory.CategoryName.Contains(cvm.CategoryName)
                      ).Select(c => new
                      {
                          PublisherAccount = c.PublisherAccount,
                          HolderAccount = c.HolderAccount,
                          ProductCategoryId = c.ProductCategoryId,
                          Discount = c.Discount,
                          StatusName = c.StatusNavigation.StatusName,
                          CategoryName = c.ProductCategory.CategoryName,
                          ExpirationDate=c.ExpirationDate,
                          Status = c.Status,
                      }
                ));
                    
        }
    }
}
