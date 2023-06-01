using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;
using System.Net;
using System.Security.Principal;

namespace Shocker_Project.Controllers
{
	public class ProductsController : Controller
	{
		private readonly db_a98a02_thm101team1001Context _context;
		private readonly IWebHostEnvironment _environment;
		private readonly IConfiguration _configuration;
		public ProductsController(db_a98a02_thm101team1001Context context, IWebHostEnvironment environment, IConfiguration configuration)
		{
			_context = context;
			_environment = environment;
			_configuration = configuration;
		}
		string loginAccount = "User2";
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult GetInfo(string id)
		{
			var p = _context.Products.AsNoTracking().Where(p => p.SellerAccount == id);			
			var od = _context.OrderDetails.AsNoTracking().Where(od => od.Product.SellerAccount == id);			
			var r = _context.Ratings.AsNoTracking().Where(r => r.Product.SellerAccount == id);
			var info = _context.Users.AsNoTracking().Where(u => u.Id == id)
					   .Select(u => new {
						   AboutSeller = u.AboutSeller,
						   p0 = p.Count(p => p.Status == "p0"),
						   p1 = p.Count(p => p.Status == "p1"),
						   od1 = od.Count(od => od.Status == "od1"),
						   od2 = od.Count(od => od.Status == "od2"),
						   od3 = od.Count(od => od.Status == "od3"),
						   od4 = od.Count(od => od.Status == "od4"),
						   r0 = r.Count(r => r.Status == "r0"),
						   r1 = r.Count(r => r.Status == "r1")
					   });					   			
			return Json(info);
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> UpdateInfo([FromBody]AboutModel info)
		{
			if (ModelState.IsValid)
			{
				Users? user = await _context.Users.FindAsync(info.Id);
				if (user == null || user.Id != loginAccount)
				{
					return Json(new { Result = "Error", Message = "記錄不存在" });
				}
				user.AboutSeller = info.AboutSeller;
				_context.Update(user);
				await _context.SaveChangesAsync();
				return Json(new { Result = "OK", Message = "修改記錄成功" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "修改記錄失敗" });
			}
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult GetProducts()
		{
			var product = from p in _context.Products
						  //where p.SellerAccount == loginAccount
						  select new Products
						  {
							  ProductId = p.ProductId,
							  SellerAccount = p.SellerAccount,
							  LaunchDate = p.LaunchDate,
							  ProductName = p.ProductName,
							  ProductCategoryId = p.ProductCategoryId,
							  Description = p.Description,
							  UnitsInStock = p.UnitsInStock,
							  Sales = p.Sales,
							  UnitPrice = p.UnitPrice,
							  Status = p.Status,
							  Currency = p.Currency
						  };
			return Json(product);
		}
		[HttpGet]
		public JsonResult GetProduct(int id)
		{
			var product = _context.Products.Where(p => p.ProductId == id)
			.Select(p => new
			{
				ProductId = p.ProductId,
				SellerAccount = p.SellerAccount,
				LaunchDate = p.LaunchDate,
				ProductName = p.ProductName,
				ProductCategory = p.ProductCategory.CategoryName,
				Description = p.Description,
				UnitsInStock = p.UnitsInStock,
				Sales = p.Sales,
				UnitPrice = p.UnitPrice,
				Status = p.Status,
				Currency = p.Currency
			});			
			return Json(product);
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult Filter([FromBody] ProductsViewModel product)
		{
			var query = _context.Products.Where(p =>
				//p.SellerAccount == loginAccount && (
				p.ProductId == product.ProductId ||
				p.ProductName.Contains(product.ProductName) ||
				p.Description.Contains(product.Description) ||
				p.StatusNavigation.StatusName.Contains(product.Status) ||
				p.ProductCategory.CategoryName.Contains(product.Description)
				//)
				).Select(p => new Products
				{
					ProductId = p.ProductId,
					SellerAccount = p.SellerAccount,
					LaunchDate = p.LaunchDate,
					ProductName = p.ProductName,
					ProductCategoryId = p.ProductCategoryId,
					Description = p.Description,
					UnitsInStock = p.UnitsInStock,
					Sales = p.Sales,
					UnitPrice = p.UnitPrice,
					Status = p.Status,
					Currency = p.Currency
				});
			return Json(query);
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> Create([FromBody] ProductsViewModel product)
		{
			if (ModelState.IsValid)
			{
				Products p = new Products();
				p.ProductId = 0;
				p.SellerAccount = product.SellerAccount;
				p.LaunchDate = DateTime.Now;
				p.ProductName = product.ProductName;
				p.ProductCategoryId = product.ProductCategoryId;
				p.Description = product.Description;
				p.UnitsInStock = product.UnitsInStock;
				p.Sales = 0;
				p.UnitPrice = product.UnitPrice;
				p.Status = product.Status;
				p.Currency = product.Currency;
				_context.Products.Add(p);
				await _context.SaveChangesAsync();
				return Json(new { Result = "OK", Record = product });
			}
			else
			{
				return Json(new { Result = "Error", Message = "新增失敗" });
			}
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> Update([FromBody] ProductsViewModel product)
		{
			if (ModelState.IsValid)
			{
				Products? p = await _context.Products.FindAsync(product.ProductId);
				if (p == null || p.SellerAccount != product.SellerAccount)
				{
					return Json(new { Result = "Error", Message = "記錄不存在" });
				}
				if (p.Status == "p0" && product.Status == "p1")
				{
					p.LaunchDate = DateTime.Now;
				}
				p.ProductName = product.ProductName;
				p.ProductCategoryId = product.ProductCategoryId;
				p.Description = product.Description;
				p.UnitsInStock = product.UnitsInStock;
				p.UnitPrice = product.UnitPrice;
				p.Status = product.Status;
				p.Currency = product.Currency;
				_context.Update(p);
				await _context.SaveChangesAsync();
				return Json((new { Result = "OK", Message = "修改記錄成功" }));
			}
			else
			{
				return Json(new { Result = "Error", Message = "修改記錄失敗" });
			}
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> Delete(int id)
		{
			Products? product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return Json(new { Result = "Error", Message = "找不到欲刪除記錄" });
			}
			try
			{
				product.Status = "p2";
				_context.Update(product);               //--Delete Pictures from Folder
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return Json(new { Result = "Error", Message = "刪除失敗" });
			}
			return Json(new { Result = "OK", Message = "刪除成功" });
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult GetPictures(int id)
		{
			var picture = from p in _context.Pictures
						  where p.ProductId == id
						  select new
						  {
							  PictureId = p.PictureId,
							  ProductId = p.ProductId,
							  Path = p.Path,
							  Description = p.Description
						  };
			return Json(picture);
		}
		[HttpPost]
		public async Task<JsonResult> UpdatePicture([FromBody] Pictures picture)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Pictures? p = await _context.Pictures.FindAsync(picture.PictureId);
					if (p == null || p.ProductId != picture.ProductId)
					{
						return Json(new { Results = "Error", Message = "記錄不存在" });
					}
					p.Description = picture.Description;
					_context.Update(p);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PictureExists(picture.PictureId))
					{
						return Json(new { Result = "Error", Message = "記錄不存在" });
					}
					else
					{
						throw;
					}
				}
				return Json((new { Result = "OK", Message = "修改記錄成功" }));
			}
			else
			{
				return Json(new { Result = "Error", Message = "修改記錄失敗" });
			}
		}

		private bool PictureExists(string pictureId)
		{
			return (_context.Pictures?.Any(p => p.PictureId == pictureId)).GetValueOrDefault();
		}
		[HttpPost]
		public async Task<JsonResult> DeletePicture(string id)
		{
			Pictures? picture = await _context.Pictures.FindAsync(id);
			if (picture == null)
			{
				return Json(new { Result = "Error", Message = "找不到欲刪除記錄" });
			}
			try
			{
				_context.Pictures.Remove(picture);               //---Delete Picture from Folder
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return Json(new { Result = "Error", Message = "刪除失敗" });
			}
			return Json(new { Result = "OK", Message = "刪除成功" });
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> UploadPicture(PicturesViewModel pic)
		{
			if (ModelState.IsValid)
			{
				var root = $@"{_environment.WebRootPath}\productpictures";
				var time = DateTime.Now.Ticks;
				var unid = Guid.NewGuid().ToString();
				var path = $@"{root}\{unid}-{time}-{pic.Picture.FileName}";
				using (var st = System.IO.File.Create(path))
				{
					pic.Picture.CopyTo(st);
				}
				Pictures p = new Pictures
				{
					PictureId = $@"{unid}",
					ProductId = pic.ProductId,
					Path = $@"{time}-{pic.Picture.FileName}",
					Description = pic.Description
				};
				_context.Pictures.Add(p);
				await _context.SaveChangesAsync();
				return Json(new { Results = "OK", Message = "新增成功" });
			}
			else
			{
				return Json(new { Results = "Error", Message = "新增失敗" });
			}
		}
		[HttpPost]
		public JsonResult GetOrders(string id)
		{
			var orders = _context.Orders.Where(o => o.OrderDetails.Any(od => od.Product.SellerAccount == id)).Select(o => new
			{
				OrderId = o.OrderId,
				BuyerAccount = o.BuyerAccount,
				Address = o.Address,
				OrderDate = o.OrderDate,
				ArrivalDate = o.ArrivalDate,
				BuyerPhone = o.BuyerPhone,
				PayMethod = o.PayMethod,
				Status = o.Status
			});
			if (orders == null) return Json(new { Result = "None", Message = "沒有訂單記錄" });
			var orderDetails = _context.OrderDetails.Where(od => od.Product.SellerAccount == loginAccount)
				.GroupBy(od => od.OrderId, (order, details) => new
				{
					OrderId = order,
					ProductCount = details.Count(),
					Product = details.ToList()
				});
			return Json(new { Orders = orders, OrderDetails = orderDetails });
		}
		[HttpPost]
		public async Task<JsonResult> UpdateOrderStatus([FromBody]OrderStatusModel order)
		{
			if (ModelState.IsValid)
			{
				for (int i = 0; i < order.ProductId.Count; i++)
				{
					OrderDetails? orderDetails = await _context.OrderDetails.FindAsync(order.OrderId, order.ProductId[i]);			
					if (orderDetails == null) return Json(new { Result = "Error", Message = $"訂單中沒有商品編號{order.ProductId[i]}" });
					orderDetails.Status = (orderDetails.Status == "od1") ? "od2" : orderDetails.Status;
					_context.Update(orderDetails);
					await _context.SaveChangesAsync();
				}
				Orders? o = await _context.Orders.FindAsync(order.OrderId);
				if (o == null) return Json(new { Result = "Error", Message = $"查無訂單編號{order.OrderId}" });
				if (o.Status == "o1")
				{
					o.Status = "o2";
					_context.Update(o);
					await _context.SaveChangesAsync();
				}
				return Json(new { Result = "OK", Message = "更新狀態成功" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "更新狀態失敗" });
			}
		}
		[HttpPost]
		public JsonResult GetRatings()
		{
			var ratings = _context.Ratings.Where(r => r.Product.SellerAccount == loginAccount)
				.GroupBy(r => r.OrderId, (order, details) => new
				{
					OrderId = order,
					Product = details.ToList()
				});
			if (ratings == null) return Json(new { Result = "None", Message = "沒有評價記錄" });
			var orders = _context.Orders.Where(o => o.OrderDetails.Any(od => od.Product.SellerAccount == loginAccount))
				.Where(o => o.Ratings.Any(r => r.OrderId == o.OrderId))
				.Select(o => new
				{
					OrderId = o.OrderId,
					BuyerAccount = o.BuyerAccount,
					OrderDate = o.OrderDate,
					ArrivalDate = o.ArrivalDate,
					Status = o.Status
				});
			var orderDetails = _context.OrderDetails.Where(od => od.Product.SellerAccount == loginAccount)
				.Where(od => od.Status == "od6")
				.GroupBy(od => od.OrderId, (order, details) => new
				{
					OrderId = order,
					Product = details.ToList()
				});
			return Json(new { Ratings = ratings, Orders = orders, OrderDetails = orderDetails });
		}
		[HttpPost]
		public async Task<JsonResult> ReplyBuyer([FromBody]ReplyModel reply)
		{
			if (ModelState.IsValid)
			{
				Ratings? rating = await _context.Ratings.FindAsync(reply.ProductId, reply.OrderId);
				if (rating == null || rating.Product.SellerAccount != loginAccount)
				{
					return Json(new { Result = "Error", Message = "評價記錄不存在" });
				}
				if (rating.Reply == "")
				{
					return Json(new { Result = "Blank", Message = "無回應內容" });
				}
				if(rating.Status == "r1")
				{
					return Json(new { Result = "Error", Message = "已回應買家評價" });
				}
				rating.Reply = reply.Reply;
				rating.Status = "r1";
				_context.Update(rating);
				await _context.SaveChangesAsync();
				return Json(new { Result = "OK", Message = "評價回應成功" });
			}
			else
			{
				return Json(new { Result = "Error", Message = "評價回應失敗" });
			}
		}
	}	
}
