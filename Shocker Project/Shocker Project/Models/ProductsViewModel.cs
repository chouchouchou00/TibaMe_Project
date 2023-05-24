namespace Shocker_Project.Models
{
	public class ProductsViewModel
	{
		public int ProductId { get; set; }
		public string? SellerAccount { get; set; }
		public string? ProductName { get; set; }
		public int ProductCategoryId { get; set; }
		public string? Description { get; set; }
		public int UnitsInStock { get; set; }
		public int UnitPrice { get; set; }
		public string? Status { get; set; }
		public string? Currency { get; set; }
	}
}
