using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
		[HttpGet]//目前打網址沒東西
		public JsonResult GetAccount(string LoginAccount, UserViewModel uvm)//要接登入驗證那裡傳回的Users.Account
		{
			LoginAccount = "Admin1";
			var getaccount = from i in _context.Users.Where(a => a.Account == LoginAccount)
							 select new
							 {
								 Account = i.Account,
								 Password = i.Password,
								 Name = i.Name,
								 Gender = i.Gender,
								 BirthDate = i.BirthDate,
								 Email = i.Email,
								 Phone = i.Phone,
								 Role = i.Role,
								 RegisterDate = i.RegisterDate
							 };
			return Json(getaccount);
		}
		///User/UpdateAccount
		[HttpPost]
		public async Task<JsonResult> UpdateAccount([Bind("Account,Password,Name,Gender,BirthDate,Email")] UserViewModel uvm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Users u = await _context.Users.FindAsync(uvm.Account);
					u.Password = uvm.Password;
					u.Name = uvm.Name;
					u.Gender = uvm.Gender;
					u.BirthDate = uvm.BirthDate;
					u.Email = uvm.Email;
					_context.Update(u);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UsersExists(uvm.Account))
					{
						return Json(new { Result = "Error", Message = "此帳號不存在!" });
					}
					else
					{
						throw;
					}
				}
				return Json(new { Result = "OK"});				
			}
			else
			{
				return Json(new { Result = "OK" });
			}
		}

		private bool UsersExists(string hasaccount)
		{
			return _context.Users.Any(u => u.Account == hasaccount);
		}
	}
}
