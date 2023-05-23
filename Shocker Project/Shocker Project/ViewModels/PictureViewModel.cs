using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shocker_Project.ViewModels
{
    public class PictureViewModel
    {
		//[Display(Name = "用戶頭像")]
		//public string PicturePath { get; set; }
		[Display(Name = "圖片")]
		public IFormFile Picture { get; set; }
	}
}