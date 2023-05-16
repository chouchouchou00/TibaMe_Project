using Microsoft.AspNetCore.Mvc;

namespace Shocker_Project.Controllers
{
    public class CommonProblemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
