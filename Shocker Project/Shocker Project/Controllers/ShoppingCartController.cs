using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;
using System.Net;

namespace Shocker_Project.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly db_a98a02_thm101team1001Context _context;

        public ShoppingCartController(db_a98a02_thm101team1001Context context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
        }
        string loginAccount = "User2";
        public IActionResult Index()
        {
            int productId = 10;
            var products = _context.Products.Where(x => x.ProductId==productId).OrderByDescending(m => m.ProductId);
            return View(products);
        }

        public IActionResult Product()
        {
              return View();
        }
        [HttpPost]
        public JsonResult GetProduct()
        {
            int productId = 2;
            var products = from x in _context.OrderDetails.Where(x => x.ProductId == productId)
                           select new
                           {
                               ProductId = x.ProductId,
                               ProductName = x.ProductName,
                               UnitPrice = x.UnitPrice,
                               Quantity = x.Quantity

                           };

            //var products = _context.Pictures.Select(p =>
            //                new
            //                {
            //                    PictureId= p.PictureId,
            //                    PicturePath=p.Path,
            //                    ProductId = p.ProductId,
            //                    ProductName = p.Product.ProductName,
            //                    UnitPrice = p.Product.UnitPrice,

            //                }
            //                );

            return Json(products);
        }
        public IActionResult ShoppingCart()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetShopping(/*[FromBody] ShoppingViewModel shoppingViewModel*/)
        {
               var d = _context.Shopping.Select(p => new
                {
                    Quantity= p.Quantity ,
                    ProductName = p.Product.ProductName,
                    ProductId =p.ProductId,
                    UnitPrice=p.Product.UnitPrice,
                });
                //_context.Shopping.Add(d);
                //await _context.SaveChangesAsync();
                //return Json(new { Result = "OK", Record = shoppingViewModel });
                return Json(d);
            }
        //    else
        //    {
        //        return Json(new { Result = "Error", Message = "新增失敗" });
        //    }
        //}
        string loginaccount = "User2";
       


        public IActionResult/*async Task<JsonResult>*/ AddCar([FromBody] ShoppingViewModel shoppingViewmodel)
        {
            if (ModelState.IsValid)
            {
                string UserId = loginaccount;//User.Identity.Name;
                var currentcar = _context.Shopping.Where(m => m.ProductId == shoppingViewmodel.ProductId && m.BuyerAccount == UserId).FirstOrDefault();
                if (currentcar == null)
                {
                    var product = _context.Shopping.Where(m => m.ProductId == shoppingViewmodel.ProductId).FirstOrDefault();
                    Shopping shopping = new Shopping();
                    shopping.ProductId = shoppingViewmodel.ProductId;
                    shopping.Quantity = 1;
                    _context.Shopping.Add(shopping);
                }
                else
                {
                    currentcar.Quantity += 1;
                }
                _context.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("ShoppingCart");
            }
        }
        public IActionResult Delete(int Id)
        {
            var shopping = _context.Shopping.Where(m => m.ProductId == 1).FirstOrDefault();
            _context.Shopping.Remove(shopping);
            _context.SaveChanges();
            return RedirectToAction("ShoppingCart");
        }


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
                order.Status = "o1";
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
                    item.Status = "o1";
                }
                _context.SaveChanges();
                return RedirectToAction("OrderList");
            }
            return View(ordersViewModel);
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
