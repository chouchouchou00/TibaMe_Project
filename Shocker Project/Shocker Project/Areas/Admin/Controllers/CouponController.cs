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
            return Json(new { Message = "回復完畢" });
        }
    }
}
