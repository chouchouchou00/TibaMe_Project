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
		private string loginAccount= "Admin1";//暫時寫死，等登入的參數
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
		public JsonResult GetAccount()//要接登入驗證那裡傳回的Users.Account
		{
			var getaccount = from u in _context.Users.Where(a => a.Account == loginAccount)//暫時寫死，等登入的參數
                             select new
							 {
								 account = u.Account,
								 password = u.Password,
								 name = u.Name,
								 gender = u.Gender,
								 birthDate = u.BirthDate,
								 email = u.Email,
								 phone = u.Phone,
								 role = u.Role,
								 registerDate = u.RegisterDate,
								 picture = u.PicturePath
							 };
			return Json(getaccount);
		}
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
				return Json(new { Result = "OK", Message = "修改成功!" });
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
				try
				{
					Users u = await _context.Users.FindAsync(pvm.Account);
					var root = $@"{_environment.WebRootPath}\img\userphoto";
					var time = DateTime.Now.Ticks;
					var unid = Guid.NewGuid();
					var path = $@"{root}\{unid}-{time}-{pvm.Picture.FileName}";
					using (var st = System.IO.File.Create(path))
					{
						pvm.Picture.CopyTo(st);
					}
					u.PicturePath = $@"img\userphoto\{unid}-{time}-{pvm.Picture.FileName}";
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UsersExists(pvm.Account))
					{
						return Json(new { Result = "Error", Message = "此帳號不存在!" });
					}
					else
					{
						throw;
					}
				}
				return Json(new { Result = "OK", Message = "上傳成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "上傳失敗!" });
			}
		}
		///User/GetOrders
		[HttpGet]
		public JsonResult GetOrders()
		{
			var getorders = from o in _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.ProductCategory).Where(a => a.BuyerAccount== loginAccount)//暫時寫死，等登入的參數
                            select new
							 {
								buyerAccount=o.BuyerAccount,
								orderId=o.OrderId,
								address=o.Address,
								orderDate=o.OrderDate,
								requiredDate=o.RequiredDate,
								buyerPhone=o.BuyerPhone,
								payMethod=o.PayMethod,
								productId = o.OrderDetails.Select(od=>od.ProductId).ToList(),
								quantity=o.OrderDetails.Select(od=>od.Quantity).ToList(),
								odstatus= o.OrderDetails.Select(od => od.Status).ToList(),
								sellerAccount=o.OrderDetails.Select(od => od.Product.SellerAccount).ToList(),
								productName=o.OrderDetails.Select(od => od.Product.ProductName).ToList(),
								productCategoryId=o.OrderDetails.Select(od => od.Product.ProductCategoryId).ToList(),
								unitPrice = o.OrderDetails.Select(od => od.Product.UnitPrice).ToList(),
								categoryName=o.OrderDetails.Select(od => od.Product.ProductCategory.CategoryName).ToList(),
							};
			return Json(getorders);
		}
		///User/UserOrderDetails
		[HttpGet]
        public async Task<IActionResult> UserOrderDetails(int? id)
		{
			id = 19;
            var getorderdetails = from o in _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.ProductCategory).Where(a => a.OrderId == id)
                            select new
                            {
                                buyerAccount = o.BuyerAccount,
                                orderId = o.OrderId,
                                address = o.Address,
                                orderDate = o.OrderDate,
                                requiredDate = o.RequiredDate,
                                buyerPhone = o.BuyerPhone,
                                payMethod = o.PayMethod,
                                productId = o.OrderDetails.Select(od => od.ProductId).ToList(),
                                quantity = o.OrderDetails.Select(od => od.Quantity).ToList(),
                                odstatus = o.OrderDetails.Select(od => od.Status).ToList(),
                                sellerAccount = o.OrderDetails.Select(od => od.Product.SellerAccount).ToList(),
                                productName = o.OrderDetails.Select(od => od.Product.ProductName).ToList(),
                                productCategoryId = o.OrderDetails.Select(od => od.Product.ProductCategoryId).ToList(),
                                unitPrice = o.OrderDetails.Select(od => od.Product.UnitPrice).ToList(),
                                categoryName = o.OrderDetails.Select(od => od.Product.ProductCategory.CategoryName).ToList(),
                            };
            var result = await Task.FromResult(getorderdetails);
            return View(result);
		}
		///Get/Coupons
		[HttpGet]
		//public JsonResult GetCoupons()
		//{
		//	var getcoupons = from c in _context.Coupons.Where(c => c.HolderAccount == loginAccount)
		//					 select new
		//					 {
		//						 holderAccount=c.HolderAccount,
		//						 exp
		//					 };
		//}
		private bool UsersExists(string hasaccount)
		{
			return _context.Users.Any(u => u.Account == hasaccount);
		}
	}
}
