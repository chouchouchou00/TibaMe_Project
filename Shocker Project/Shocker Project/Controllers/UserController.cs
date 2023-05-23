using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Shocker_Project.Models;
using Shocker_Project.ViewModels;
using System.Net.Sockets;
using System.Security.Principal;

namespace Shocker_Project.Controllers
{
    [Route("{controller}/{action}/{id?}")]
	public class UserController : Controller
	{
		private readonly db_a98a02_thm101team1001Context _context;
		private readonly IWebHostEnvironment _environment;

		public UserController(db_a98a02_thm101team1001Context context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}
		[HttpGet]
		public async Task<IActionResult> MyAccount(string tab)
		{
			ViewBag.Tab = tab;
			return View();
		}
		///User/GetAccount
		[HttpGet]
		public JsonResult GetAccount(string LoginAccount)//要接登入驗證那裡傳回的Users.Account
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
								 registerDate = i.RegisterDate,
								 picture=i.PicturePath
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
				return Json(new { Result = "OK", Message ="修改成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "修改失敗!" });
			}
		}
		[HttpPost]
		public async Task<JsonResult> Uploadpicture(PictureViewModel pvm)
		{
			if (ModelState.IsValid)
			{
				var root=$@"{_environment.WebRootPath}\img\userphoto";
				var time = DateTime.Now.Ticks;
				var unid=Guid.NewGuid();
				var path = $@"{root}\{unid}-{time}-{pvm.Picture.FileName}";
				using(var st = System.IO.File.Create(path))
				{
					pvm.Picture.CopyTo(st);
				}
				await _context.SaveChangesAsync();
				return Json(new { Result = "OK",Message = "上傳成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "上傳失敗!" });
			}
		}
		private bool UsersExists(string hasaccount)
		{
			return _context.Users.Any(u => u.Account == hasaccount);
		}

		//public IActionResult Uploadpicture(IFormFile pic, string ad)
		//{
		//	//DateTime.Now.Ticks:精細記下當時的時間
		//	using (var stream = System.IO.File.Create("C:\Users\Tibame_T14\Desktop\第一組專題\Shocker Project\Shocker Project\wwwroot\img"){ ad}
		//	{ pic.FileName})
		//	{
		//		pic.CopyTo(stream);
		//	}
		//	return Ok("true");
		//}
	}
}
