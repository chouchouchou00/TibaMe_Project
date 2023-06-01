using Microsoft.AspNetCore.Mvc;
using Shocker_Project.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


namespace Shocker_Project.Areas.Shoppingcart.Controllers
{
    public class HomeController : Controller
    {
        db_a98a02_thm101team1001Context db = new db_a98a02_thm101team1001Context();
        public IActionResult Index()
        {
            var products = db.Products.OrderByDescending(m => m.ProductId).ToList();
            return View(products);
        }
    }
}
