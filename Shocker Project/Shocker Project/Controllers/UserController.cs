using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Shocker_Project.Models;
using Shocker_Project.ViewModels;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Security.Principal;

namespace Shocker_Project.Controllers
{
	[Route("{controller}/{action}/{id?}")]
	public class UserController : Controller
	{
		private string loginAccount = "Admin1";//暫時寫死，等登入的參數
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
			var getaccount = from u in _context.Users.AsNoTracking().Where(a => a.Account == loginAccount)//暫時寫死，等登入的參數
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
				return Json(new { Result = "OK", Message = "修改帳號資料成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "修改帳號資料失敗!" });
			}
		}
		[HttpPost]
		public async Task<JsonResult> UploadPicture(PictureViewModel pvm)
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
				return Json(new { Result = "OK", Message = "上傳照片成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "上傳照片失敗!" });
			}
		}
		///User/GetOrders
		[HttpGet]
		public JsonResult GetOrders()
		{
			var getorders = from o in _context.Orders.AsNoTracking().Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.ProductCategory).Where(a => a.BuyerAccount == loginAccount)//暫時寫死，等登入的參數
							select new
							{
								buyerAccount = o.BuyerAccount,
								orderId = o.OrderId,
								address = o.Address,//沒用到
								orderDate = o.OrderDate,//沒用到
								arrivalDate = o.ArrivalDate,//沒用到
								buyerPhone = o.BuyerPhone,//沒用到
								payMethod = o.PayMethod,
								quantity = o.OrderDetails.Select(od => od.Quantity).ToList(),
								productName = o.OrderDetails.Select(od => od.ProductName).ToList(),
								unitPrice = o.OrderDetails.Select(od => od.UnitPrice).ToList(),
								statusName = o.StatusNavigation.StatusName,
							};
			return Json(getorders);
		}
		//useless:
		//								productId
		//								productCategoryId
		//								sellerAccount
		//								categoryName
		[HttpPost]
		public async Task<JsonResult> CancelOrders([FromBody] CancelOrdersViewModel covm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Orders o = await _context.Orders.Include(o => o.OrderDetails).Where(o => o.OrderId == covm.OrderId).FirstOrDefaultAsync();
					o.Status = "o5";
					foreach (var od in o.OrderDetails)
					{
						od.Status = "od5";
					}
					_context.Update(o);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrdersExists(covm.OrderId))
					{
						return Json(new { Result = "Error", Message = "此訂單不存在!" });
					}
					else
					{
						throw;
					}
				}
				return Json(new { Result = "OK", Message = "取消訂單成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "取消訂單失敗!" });
			}			
		}		
		public IActionResult UserOrderDetails(int id)//全丟抓參數
		{
			return View();
		}
		///User/GetUserOrderDetails
		[HttpGet]
		public JsonResult GetUserOrderDetails()
		{
			var getorderdetails = from o in _context.Orders.AsNoTracking().Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Pictures).Where(a => a.OrderId == 19)
								  select new
								  {
									  buyerAccount = o.BuyerAccount,
									  orderId = o.OrderId,
									  address = o.Address,
									  orderDate = o.OrderDate,
									  arrivalDate = o.ArrivalDate,
									  buyerPhone = o.BuyerPhone,
									  payMethod = o.PayMethod,
									  productId = o.OrderDetails.Select(od => od.ProductId).ToList(),
									  quantity = o.OrderDetails.Select(od => od.Quantity).ToList(),
									  sellerAccount = o.OrderDetails.Select(od => od.Product.SellerAccount).ToList(),
									  productName = o.OrderDetails.Select(od => od.ProductName).ToList(),
									  unitPrice = o.OrderDetails.Select(od => od.UnitPrice).ToList(),
									  categoryName = o.OrderDetails.Select(od => od.Product.ProductCategory.CategoryName).ToList(),
									  path = o.OrderDetails.Select(od => od.Product.Pictures.Select(p => p.Path)).ToList(),
									  statusName = o.OrderDetails.Select(od => od.StatusNavigation.StatusName),
									  description = o.Ratings.Select(r => r.Description).ToList(),
									  starCount = o.Ratings.Select(od => od.StarCount).ToList(),
								  };
			return Json(getorderdetails);
		}
		[HttpPost]
		public async Task<JsonResult> TakeProduct([FromBody] TakeProductViewModel tpvm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					OrderDetails od = await _context.OrderDetails.Where(od => od.OrderId == tpvm.OrderId && od.ProductId == tpvm.ProductId).FirstOrDefaultAsync();
					od.Status = "od3";
					_context.Update(od);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrderDetailsExists(tpvm.OrderId, tpvm.ProductId))
					{
						return Json(new { Result = "Error", Message = "此訂單明細不存在!" });
					}
					else
					{
						throw;
					}
				}
				return Json(new { Result = "OK", Message = "更新訂單明細狀態成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "更新訂單明細狀態失敗!" });
			}
		}
		[HttpPost]
		public async Task<JsonResult> CreateRating([FromBody] RatingViewModel rvm)
		{
			if (ModelState.IsValid)
			{
				Ratings r = new Ratings
				{
					ProductId = rvm.ProductId,
					StarCount = rvm.StarCount,
					OrderId = rvm.OrderId,
				};
				OrderDetails od = await _context.OrderDetails.Where(od => od.OrderId == rvm.OrderId && od.ProductId == rvm.ProductId).FirstOrDefaultAsync();
				od.Status = "od6";
				_context.Ratings.Add(r);
				_context.Update(od);
				await _context.SaveChangesAsync();
				return Json(new { Result = "OK", Message = "新增評價成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "新增評價失敗!" });
			}
		}
		[HttpPost]
		public async Task<JsonResult> UpdateOdReturnreason([FromBody] ReturnreasonViewModel rrvm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					OrderDetails od = await _context.OrderDetails.Where(od => od.OrderId == rrvm.OrderId && od.ProductId == rrvm.ProductId).FirstOrDefaultAsync();
					od.ReturnReason = rrvm.ReturnReason;
					od.Status = "od4";
					Orders o = await _context.Orders.Where(o => o.OrderId == rrvm.OrderId).FirstOrDefaultAsync();
					o.Status = "o4";
					_context.Update(od);
					_context.Update(o);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OrderDetailsExists(rrvm.OrderId, rrvm.ProductId))
					{
						return Json(new { Result = "Error", Message = "此訂單明細不存在!" });
					}
					else
					{
						throw;
					}
				}
				return Json(new { Result = "OK", Message = "上傳訂單明細評價成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "上傳訂單明細評價失敗!" });
			}
		}
		[HttpPost]
		public async Task<JsonResult> UpdateRatingDescription([FromBody] RatingDescriptionViewModel rdvm)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Ratings r = await _context.Ratings.Where(r => r.OrderId == rdvm.OrderId && r.ProductId == rdvm.ProductId).FirstOrDefaultAsync();
					r.Description = rdvm.Description;
					_context.Update(r);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RatingsExists(rdvm.OrderId, rdvm.ProductId))
					{
						return Json(new { Result = "Error", Message = "此訂單明細不存在!" });
					}
					else
					{
						throw;
					}
				}
				return Json(new { Result = "OK", Message = "上傳訂單明細成功!" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "上傳訂單明細失敗!" });
			}
		}



		///Get/Coupons
		//[HttpGet]
		//public JsonResult GetCoupons()
		//{
		//	var getcoupons = from c in _context.Coupons.Where(c => c.HolderAccount == loginAccount)
		//					 select new
		//					 {
		//						 holderAccount = c.HolderAccount,
		//						 exp
		//					 };
		//}
		private bool OrdersExists(int hasorderId)
		{
			return _context.Orders.Any(o => o.OrderId == hasorderId);
		}
		private bool OrderDetailsExists(int hasorderId, int hasproductId)
		{
			return _context.OrderDetails.Any(od => od.OrderId == hasorderId && od.ProductId == hasproductId);
		}
		private bool UsersExists(string hasaccount)
		{
			return _context.Users.Any(u => u.Account == hasaccount);
		}
		private bool RatingsExists(int hasorderId, int hasproductId)
		{
			return _context.Ratings.Any(r => r.OrderId == hasorderId && r.ProductId == hasproductId);
		}
	}
}
