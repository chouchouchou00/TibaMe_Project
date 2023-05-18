using Microsoft.AspNetCore.Mvc;
using Shocker_Project.Models;

namespace Shocker_Project.Controllers
{
    [Route("{controller}/{action}/{id?}")]
    public class UserController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public UserController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }
        [HttpGet]
        
        public IActionResult MyAccount(string tab)
        {
            ViewBag.Tab = tab;
            return View();

        }


		//var Myaccountinformation = from i in _context.Users
		//						   where i.Account == Account
		//						   select new
		//						   {
		//							   Account=i.Account, //Readonly
		//							   Password=i.Password,
		//							   Name=i.Name,
		//							   Gender=i.Gender,
		//							   BirthdDate=i.BirthDate,
		//							   Email=i.Email,
		//							   Phone=i.Phone,
		//							   Role=i.Role, //Readonly
		//							   RegisterDate=i.RegisterDate//Readonly
		//						   };
		//return Json(new { Result = "OK", Records = "Users" });
		//public IActionResult Index()
		//{
		//	return View();
		//}
	}
}
