using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		///User/GetAccount
		[HttpGet]
		public JsonResult GetAccount(/*string Account*/)
		{
			var getAccount = from i in _context.Users
								 //where i.Account==Account
							 select new
							 {
								 Account = i.Account,//Readonly
								 Password = i.Password,
								 Name = i.Name,
								 Gender = i.Gender,
								 BirthDate = i.BirthDate,//Readonly
								 Email = i.Email,
								 Phone = i.Phone,
								 Role = i.Role, //Readonly
								 RegisterDate = i.RegisterDate//Readonly
							 };
			return Json(getAccount);
		}
		///User/UpdateAccount
		//[HttpPost]
		//public async Task<JsonResult> UpdateAccount([Bind("Account,Password,Name,Gender,BirthDate,Email")] UserViewModel updateusers)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			Users u = await _context.Users.FindAsync(updateusers.Account);
		//			u.Password = updateusers.Password;
		//			u.Name = updateusers.Name;
		//			u.Gender = updateusers.Gender;
		//			u.BirthDate = updateusers.BirthDate;
		//			u.Email = updateusers.Email;
		//			_context.Update(u);
		//			await _context.SaveChangesAsync();
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			if (!UsersExists(user))

		//		}

		//	}
		//}
	}
}
