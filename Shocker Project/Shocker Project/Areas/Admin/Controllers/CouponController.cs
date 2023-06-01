using Microsoft.AspNetCore.Components.Web;
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
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponsViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                for (var i = 1; i < cvm.Amount; i++)
                {
                    //cvm.PublisherAccount = "Admin1";
                    Coupons coupons = new Coupons()
                    {
                        //CouponId = 0,
                        ExpirationDate = cvm.ExpirationDate,
                        HolderAccount = cvm.HolderAccount,
                        ProductCategoryId = cvm.ProductCategoryId,
                        Discount = cvm.Discount,
                        Status = cvm.Status,
                        PublisherAccount = cvm.PublisherAccount,

                    };
                    try
                    {
                        _context.Coupons.Add(coupons);
                        await _context.SaveChangesAsync();
                    }

                    catch (Exception)
                    {
                        return BadRequest("錯誤");
                    }
                }
                return Json(new { Message = "成功傳入資料庫" });
            }

            else { return Json(new { Message = "未傳至後端" }); }
            
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
