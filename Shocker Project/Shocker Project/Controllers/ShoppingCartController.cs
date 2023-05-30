using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;
using System.Net;

namespace Shocker_Project.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly db_a98a02_thm101team1001Context _context;

        public ShoppingCartController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        // GET: Shoppingcart/ShoppingCartProducts
        
        [HttpPost]
        [ValidateAntiForgeryToken]


        //private bool ProductsExists(int id)
        //{
        //    return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        //}
        public IActionResult Product()
        {
            return View();
        }
        string loginaccount = "User2";
        public IActionResult ShoppingCart()
        {            
            string UserId = loginaccount; //User.Identity.Name;
            var Shopping = _context.Shopping.Where(m => m.BuyerAccount == UserId/* &&*/ /*m.Status == "購物車"*/).ToList();
            return View(Shopping);
        }
        [HttpGet]
        public async Task<Shopping>GetShopping(int id) 
        {
            var shopping = await _context.Shopping.FindAsync(id);
            Shopping shopping1 = new Shopping
            {
                BuyerAccount = shopping.BuyerAccount,
                ProductId = shopping.ProductId,
                Quantity = shopping.Quantity,
            };
            return shopping1;
        }
        //[HttpPut]
        //public async Task<string> PutShopping(string BuyerAccount, int ProductId, int Quantity)
        //{
        //    if()
        //}
        //[HttpPost]
        //public async Task<ActionResult<Shopping>>PostShopping(Shopping)
        //{
        //    _context.Shopping

        //}
        //[HttpDelete]


        //public /*IActionResult*/async Task<JsonResult> AddCar([FromBody] ShoppingViewModel shoppingViewmodel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string UserId = loginaccount;//User.Identity.Name;
        //        var currentcar = _context.Shopping.Where(m => m.ProductId == UserId && m.BuyerAccount == UserId).FirstOrDefault();
        //        if (currentcar == null)
        //        {
        //            var product = await _context.Products.Where(m => m.ProductId == ProductId).FirstOrDefault();
        //            Shopping shopping = new Shopping();
        //            shopping.ProductId = ProductId;
        //            shopping.Quantity = 1;
        //            _context.Shopping.Add(shopping);
        //        }
        //        else
        //        {
        //            currentcar.Quantity += 1;
        //        }
        //        _context.SaveChanges();
        //        //return View();
        //    }
        //    else
        //    {
        //        return Json(new { Results = "Error", Message = "新增失敗" });
        //    }
        //}
        //public IActionResult Delete(int Id)
        //{
        //    var shopping = _context.Shopping.Where(m => m.ProductId == ProductId).FirstOrDefault();
        //    _context.Shopping.Remove(shopping);
        //    _context.SaveChanges();
        //    return RedirectToAction("ShoppingCart");
        //}


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(OrdersViewModel ordersViewModel)
        {

            if (ModelState.IsValid)
            {
                string UserId = loginaccount;//User.Identity.Name;
                                             //string guid = Guid.NewGuid().ToString();
                Orders order = new Orders();
                order.BuyerAccount = UserId;
                order.Address = ordersViewModel.Address;
                order.BuyerPhone = ordersViewModel.BuyerPhone;
                order.OrderDate = DateTime.Now;
                order.PayMethod = ordersViewModel.PayMethod;
                order.Status = "未出貨";
                _context.Orders.Add(order);
                //order.BuyerAccount = "User2";
                //order.Address = "台中";
                //order.BuyerPhone = "0933335566";
                //order.OrderDate = DateTime.Now;
                //order.PayMethod = "信用卡";
                //order.Status = "未出貨";
                //order.ArrivalDate = ;
                //_context.Orders.Add(order);

                var carList = _context.Orders.Where(m => m.BuyerAccount == UserId)/*.ToList()*/;


                foreach (var item in carList)
                {
                    item.Status = "未出貨";
                }
                _context.SaveChanges();
                return RedirectToAction("OrderList");
            }
            return View();
        }


        //建立訂單主檔列表
        public IActionResult OrderList()
        {
            string UserId = loginaccount;//User.Identity.Name;
            var orders = _context.Orders.Where(m => m.BuyerAccount == UserId).OrderByDescending(m => m.OrderDate).ToList();
            //目前會員的訂單主檔OrderList
            return View(orders);
        }
        public IActionResult OrderDetails(int OrderId)
        {
            var orderDetails = _context.OrderDetails.Where(m => m.OrderId == OrderId).ToList();
            return View(orderDetails);
        }
      
    }
}
