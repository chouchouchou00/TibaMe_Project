using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Areas.Shoppingcart.Views.ViewModel;
using Shocker_Project.Models;
using Shocker_Project.Models.ViewModel;

namespace Shocker_Project.Areas.Shoppingcart.Controllers
{
    //[Authorize]
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


        // GET: Shoppingcart/ShoppingCartProducts/Create



        // GET: Shoppingcart/ShoppingCartProducts/Edit/5


        // POST: Shoppingcart/ShoppingCartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        

        private bool ProductsExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        public IActionResult Product()
        {
            return View();
        }
        string loginaccount = "User2";
        public IActionResult ShoppingCart(ShoppingCartOrderDetailViewModel orderDetailViewModel)
        {
            string UserId = loginaccount; //User.Identity.Name;
            var orderDetails = _context.OrderDetails.Where(m => m.Order.BuyerAccount == UserId && m.Status == "購物車").ToList();
            return View(orderDetails);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShoppingCart(/*[Bind("PayMethod,Address,BuyerPhone")]*/ ShoppingCartOrderViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                string UserId = loginaccount;//User.Identity.Name;
                                             //string guid = Guid.NewGuid().ToString();
                Orders order = new Orders();
                order.BuyerAccount = UserId;
                order.Address = viewModel.Address;
                order.BuyerPhone = viewModel.BuyerPhone;
                order.OrderDate = DateTime.Now;
                order.PayMethod = viewModel.PayMethod;
                order.Status = "未出貨";
                _context.Orders.Add(order);

                var carList = _context.OrderDetails.Where(m => m.Status == "購物車" && m.Order.BuyerAccount == UserId)/*.ToList()*/;


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
        public IActionResult AddCar(int ProductId)
        {
            string UserId = loginaccount;//User.Identity.Name;
            var currentcar = _context.OrderDetails.Where(m => m.ProductId == ProductId && m.Status == "購物車" && m.Order.BuyerAccount == UserId).FirstOrDefault();
            if (currentcar == null)
            {
                var product = _context.Products.Where(m => m.ProductId == ProductId).FirstOrDefault();
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.ProductId = ProductId;
                orderDetails.Quantity = 1;
                orderDetails.Status = "購物車";
                _context.OrderDetails.Add(orderDetails);
            }
            else
            {
                currentcar.Quantity += 1;
            }
            _context.SaveChanges();
            return RedirectToAction("ShoppingCart");
        }
        public IActionResult Delete(int OrderId)
        {
            var orderDetail = _context.OrderDetails.Where(m => m.OrderId == OrderId).FirstOrDefault();
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
            return RedirectToAction("ShoppingCart");
        }
    }
}
