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
		[HttpGet]
		public JsonResult GetAccount(string LoginAccount, UserViewModel uvm)//要接登入驗證那裡傳回的Users.Account
		{
			LoginAccount = "Admin1";
			var getaccount = from i in _context.Users.Where(a => a.Account == LoginAccount)
							 select new
							 {
								 account = i.Account,
								 password = i.Password,
								 name = i.Name,
								 gender = i.Gender,
								 birthDate = i.BirthDate,
								 email = i.Email,
								 phone = i.Phone,
								 role = i.Role,
								 registerDate = i.RegisterDate
							 };
			return Json(getaccount);
		}
		///User/UpdateAccount
		[HttpPost]
		public async Task<JsonResult> UpdateAccount([FromBody] UserViewModel uvm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Users u = await _context.Users.FindAsync(uvm.Account);
					u.Account = uvm.Account;
					u.Password = uvm.Password;
					u.Name = uvm.Name;
					u.Gender = uvm.Gender;
					u.BirthDate = uvm.BirthDate;
					u.Email = uvm.Email;
					u.Phone = uvm.Phone;
					u.Role = uvm.Role;
					u.RegisterDate = uvm.RegisterDate;
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
				return Json(new { Result = "OK" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "修改失敗!" });
			}
		}

		private bool UsersExists(string hasaccount)
		{
			return _context.Users.Any(u => u.Account == hasaccount);
		}
		//public IActionResult uploadpic(IFormFile pic,string ad)
		//{
		//	//DateTime.Now.Ticks:精細記下當時的時間
		//	using (var stream = System.IO.File.Create(圖片欲存路徑){ad}
		//	{ pic.FileName})
		//	{
		//		pic.CopyTo(stream);
		//	}
		//	return Ok("true");
		//}
	}
}
