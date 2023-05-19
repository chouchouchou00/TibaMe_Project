using Microsoft.AspNetCore.Mvc;
using Shocker_Project.Models;
using Shocker_Project.ViewModels;
using System.Security.Principal;

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
        public async Task<IActionResult> MyAccount(string tab)
        {

            ViewBag.Tab = tab;

            return View();

        }
		///User/MyAccountInformation
		[HttpGet]
        public JsonResult MyAccountInformation()
        {
            var Myaccountinformation = from i in _context.Users
                                           //.Where(users => users.Account == userviewmodel.Account)
                                       select new
                                       {
                                           Account = i.Account,
                                           Password = i.Password,
                                           Name = i.Name,
                                           Gender = i.Gender,
                                           BirthDate = i.BirthDate,//Readonly
                                           Email = i.Email,
                                           Phone = i.Phone,
                                           Role = i.Role, //Readonly
                                           RegisterDate = i.RegisterDate//Readonly
                                       };
            return Json(Myaccountinformation);
        }    
	}
}
