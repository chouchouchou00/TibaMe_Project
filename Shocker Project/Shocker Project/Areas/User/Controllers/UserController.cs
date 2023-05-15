using Microsoft.AspNetCore.Mvc;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.User.Controllers
{
	[Area("User")]
	[Route("{area}/{controller}/{action}/{id?}")]
	public class UserController : Controller
	{		
		private readonly db_a98a02_thm101team1001Context _context;

		public UserController(db_a98a02_thm101team1001Context context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult MyAccount()
		{
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
			return View();
		}
        public IActionResult EditMyAccount()
		{
            return PartialView();
        }
        public IActionResult MyPayment()
		{
			return PartialView();
		}
		public IActionResult Orderlists()
		{
            return PartialView();
        }
		public IActionResult MyPromocode()
		{
            return PartialView();
        }
		public IActionResult QuestionRecord()
		{
            return PartialView();
        }

		//public IActionResult Index()
		//{
		//	return View();
		//}
	}
}
