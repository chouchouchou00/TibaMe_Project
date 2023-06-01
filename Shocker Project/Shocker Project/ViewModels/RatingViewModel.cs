using System.ComponentModel.DataAnnotations;
namespace Shocker_Project.ViewModels
{
	public class RatingViewModel
	{
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		//public string Description { get; set; }
		public int StarCount { get; set; }
	}
}
