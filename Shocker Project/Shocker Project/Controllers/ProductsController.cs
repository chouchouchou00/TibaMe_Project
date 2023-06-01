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
		string loginAccount = "User1";
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult GetInfo() //(string account)
		{
			var info = from u in _context.Users
					   where u.Id == loginAccount //== account
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
			var sellerinfo = _context.Users.Where(u => u.Id == loginAccount)
				.Select(u => u.AboutSeller);
			return Json(sellerinfo);
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult GetProducts()//(string account)
		{
			var product = from p in _context.Products
						  //where p.SellerAccount == account
						  select new
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
		public async Task<JsonResult> Filter([FromBody] ProductsViewModel product)
		{
			var query = _context.Products.Where(
				p => /*p.SellerAccount == User.Identity.Name ||*/
					 p.ProductName.Contains(product.ProductName) ||
					 p.Description.Contains(product.Description) ||
					 p.Status.Contains(product.Status))
					 .Select(p => new Products
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
		[HttpGet]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> Create([FromBody] Products product)
		{
			if (ModelState.IsValid)
			{
				_context.Products.Add(product);
				await _context.SaveChangesAsync();
				return Json(new { Results = "OK", Record = product });
			}
			else
			{
				return Json(new { Results = "Error", Message = "新增失敗" });
			}
		}
		[HttpGet]
		//[ValidateAntiForgeryToken]
		public async Task<JsonResult> Update(ProductsViewModel product)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Products? p = await _context.Products.FindAsync(product.ProductId);
					if (p == null) return Json(new { Results = "Error", Message = "記錄不存在" });
					p.SellerAccount = product.SellerAccount;
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
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
			catch(DbUpdateException)
			{
				return Json(new { Result = "Error", Message = "刪除失敗" });
			}
			return Json(new { Result = "OK", Message = "刪除成功" });
		}
		[HttpGet]
		//[ValidateAntiForgeryToken]
		public JsonResult GetPicture(int id)
		{
			var picture = from p in _context.Pictures
						  where p.ProductId == id
						  select new Pictures
						  {
							  Path = p.Path,
							  Description = p.Description
						  };
			return Json(picture);
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
