using System.ComponentModel.DataAnnotations;
namespace Shocker_Project.ViewModels
{
	public class ReturnreasonViewModel
	{
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public string ReturnReason { get; set; }
	}
}
