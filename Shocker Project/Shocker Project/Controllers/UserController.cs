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
		private string loginAccount = "User8";//暫時寫死，等登入的參數
		private readonly db_a98a02_thm101team1001Context _context;
		private readonly IWebHostEnvironment _environment;

		public UserController(db_a98a02_thm101team1001Context context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}
		[HttpGet]
		public async Task<IActionResult> MyAccount(string tab)//點選用戶資訊編輯的菜單選項時，帶一個tab的參數，依據參數abcde呈現不同的Partial View
		{
			ViewBag.Tab = tab;
			return View();
		}
		///User/GetAccount
		[HttpGet]
		public JsonResult GetAccount()//要接登入驗證那裡傳回的Users.Account參數，找出User表裡Account欄位符合登入帳號的該筆資料，將資料物件包成JSON傳到前端，先暫時寫死，等登入的參數
		{
			var getaccount = from u in _context.Users.AsNoTracking().Where(a => a.Id == loginAccount)
							 select new
							 {
								 id = u.Id,
								 password = u.Password,
								 nickName = u.NickName,
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
		public async Task<JsonResult> UpdateAccount([FromBody] UserViewModel uvm)//更新User資訊
		{
			if (ModelState.IsValid)
			{
				try
				{
					Users u = await _context.Users.FindAsync(uvm.Id);
					u.Id = uvm.Id;
					u.Password = uvm.Password;
					u.NickName = uvm.NickName;
					u.Gender = uvm.Gender;
					u.BirthDate = uvm.BirthDate;
					u.Email = uvm.Email;
					u.Phone = uvm.Phone;
					u.Role = uvm.Role;
					u.RegisterDate = uvm.RegisterDate;
					_context.Update(u);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)//捕捉資料更新時該筆資料不存在的例外
				{
					if (!UsersExists(uvm.Id))
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
		public async Task<JsonResult> UploadPicture(PictureViewModel pvm)//更改User照片
		{
			if (ModelState.IsValid)
			{
				try
				{
					Users u = await _context.Users.FindAsync(pvm.Id);
					var root = $@"{_environment.WebRootPath}\img\userphoto";//網站根目錄的路由
					var time = DateTime.Now.Ticks;//當下的時間
					var unid = Guid.NewGuid();//創生一獨一無二的Id
					var path = $@"{root}\{unid}-{time}-{pvm.Picture.FileName}";//路徑全名:路由/檔案名
					using (var st = System.IO.File.Create(path))//創造路徑
					{
						pvm.Picture.CopyTo(st);//將前端傳來的檔案複製至該路徑上
					}
					u.PicturePath = $@"img\userphoto\{unid}-{time}-{pvm.Picture.FileName}";//擷取後半段存入資料庫的PicturePath欄位
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)//捕捉資料更新時該筆資料不存在的例外
				{
					if (!UsersExists(pvm.Id))
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
		public JsonResult GetOrders()//抓取登入的Account的所有的Orders的以下欄位資訊，暫時寫死，等登入傳入的參數
		{
			var getorders = from o in _context.Orders.AsNoTracking().Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.ProductCategory).Where(a => a.BuyerAccount == loginAccount)
							select new
							{
								buyerAccount = o.BuyerAccount,
								orderId = o.OrderId,
								payMethod = o.PayMethod,
								quantity = o.OrderDetails.Select(od => od.Quantity).ToList(),
								productName = o.OrderDetails.Select(od => od.ProductName).ToList(),
								unitPrice = o.OrderDetails.Select(od => od.UnitPrice).ToList(),
								statusName = o.StatusNavigation.StatusName,
							};
			return Json(getorders);
		}
		[HttpPost]
		public async Task<JsonResult> CancelOrders([FromBody] CancelOrdersViewModel covm)//更改Order狀態為已取消
		{
			if (ModelState.IsValid)
			{
				try
				{
					Orders o = await _context.Orders.Include(o => o.OrderDetails).Where(o => o.OrderId == covm.OrderId).FirstOrDefaultAsync();
					o.Status = "o5";//o5=已取消
					foreach (var od in o.OrderDetails)//尋覽每一個此筆Order裡的OrderDetail，並將他們的狀態一併改為已取消
					{
						od.Status = "od5";//od5=已取消
					}
					_context.Update(o);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)//捕捉資料更新時該筆資料不存在的例外
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
			ViewBag.OrderId = id;
			return View();
		}
		///User/GetUserOrderDetails
		[HttpGet]
		public JsonResult GetUserOrderDetails()//抓取上頁點選之指定OrderId的全部OrderDetail的指定欄位的資料
		{			
			var getorderdetails = from o in _context.Orders.AsNoTracking().Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Pictures)/*.Where(a => a.OrderId == 19)*/
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
									  discount = o.OrderDetails.Select(od => od.Discount).ToList(),
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
				catch (DbUpdateConcurrencyException)//捕捉資料更新時該筆資料不存在的例外
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
				catch (DbUpdateConcurrencyException)//捕捉資料更新時該筆資料不存在的例外
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
				catch (DbUpdateConcurrencyException)//捕捉資料更新時該筆資料不存在的例外
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
		///User/GetCoupons
		[HttpGet]
		public JsonResult GetCoupons()
		{
			var getcoupons = from c in _context.Coupons.Where(c => c.HolderAccount == loginAccount)
							 select new
							 {
								 holderAccount = c.HolderAccount,
								 couponId=c.CouponId,
								 expirationDate=c.ExpirationDate,
								 productCategoryName=c.ProductCategory.CategoryName,
								 discount=c.Discount,								 
								 publisherAccount=c.PublisherAccount,
								 statusName=c.StatusNavigation.StatusName,
								 orderId=c.OrderDetails.Select(o=>o.OrderId).ToList(),
							 };
			return Json(getcoupons);
		}
		///User/GetQuestions
		[HttpGet]
		public JsonResult GetQuestions()
		{
			var getquestions = _context.ClientCases.Where(c => c.UserAccount == loginAccount).Select(
				c => new
				{
					caseId = c.CaseId,
					description = c.Description,
					statusName = c.StatusNavigation.StatusName,
					reply = c.Reply,
					categoryName = c.QuestionCategory.CategoryName,
				});
			return Json(getquestions);
		}

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
			return _context.Users.Any(u => u.Id == hasaccount);
		}
		private bool RatingsExists(int hasorderId, int hasproductId)
		{
			return _context.Ratings.Any(r => r.OrderId == hasorderId && r.ProductId == hasproductId);
		}
	}
}
