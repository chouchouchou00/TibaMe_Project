using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;
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
		[ValidateAntiForgeryToken]
		public JsonResult GetInfo() //(string account)
		{
			var info = from u in _context.Users
					   where u.Account == loginAccount
					   select new
					   {
						   PicturePath = u.PicturePath,
						   AboutSeller = u.AboutSeller
					   };
			return Json(info);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult UpdateInfo(string info) //(string account)
		{
			var sellerinfo = _context.Users.Where(u => u.Account == loginAccount)
				.Select(u => u.AboutSeller);
			return Json(sellerinfo);
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
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult Filter([FromBody] ProductsViewModel product)
		{
			var query = _context.Products.Where(p =>
				//p.SellerAccount == loginAccount && (
				p.ProductName.Contains(product.ProductName) ||
				p.Description.Contains(product.Description) ||
				p.Status.Contains(product.Status) ||
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
				return Json(new { Results = "OK", Record = product });
			}
			else
			{
				return Json(new { Results = "Error", Message = "新增失敗" });
			}
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> Update([FromBody] ProductsViewModel product)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Products? p = await _context.Products.FindAsync(product.ProductId);
					if (p == null || p.SellerAccount != product.SellerAccount)
					{
						return Json(new { Results = "Error", Message = "記錄不存在" });
					}
					if (p.Status == "未上架" && product.Status == "已上架")
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
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.ProductId))
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

		private bool ProductExists(int id)
		{
			return (_context.Products?.Any(p => p.ProductId == id)).GetValueOrDefault();
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
				_context.Products.Remove(product);               //--Delete Pictures from Folder
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
	}
}
