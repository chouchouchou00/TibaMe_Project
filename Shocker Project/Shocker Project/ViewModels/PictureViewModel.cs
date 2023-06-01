using System.ComponentModel.DataAnnotations;

namespace Shocker_Project.ViewModels
{
    public class PictureViewModel
    {
		[Display(Name = "帳號名稱")]//Readonly
		public string Id { get; set; }
		[Display(Name = "圖片")]
		public IFormFile Picture { get; set; }
	}
}