using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
//using System.Web.Security;
using Shocker_Project.Models;
using System.Web.Providers.Entities;

namespace Shocker_Project.Controllers
{
    
    public class MemberController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public MemberController(db_a98a02_thm101team1001Context context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string Account, string Password)
        {
            var member = _context.Users
                .Where(m => m.Account == Account && m.Password == Password)
                .FirstOrDefault();

            if (member == null)
            {
                ViewBag.Message = "帳密有錯，登入失敗";
                return View();
            }

            //Session["Welcome"] = member.Name + "歡迎光臨";
            //FormAuthentication.RedirectFromLoginPage(Account, true);
            return Redirect("~/Home");
        }

        public IActionResult SignOut()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
