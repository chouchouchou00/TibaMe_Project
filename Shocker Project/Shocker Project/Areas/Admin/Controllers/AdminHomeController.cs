using Microsoft.AspNetCore.Mvc;

namespace Shocker_Project.Areas.Admin.Controllers
{
    [Area ("Admin")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
