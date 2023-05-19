using System.ComponentModel.DataAnnotations;

namespace Shocker_Project.ViewModels
{
    public class UserViewModel: IValidatableObject
    {
		[Display(Name = "帳號名稱")]//Readonly
		public string Account { get; set; }
		[Required(ErrorMessage = "密碼為必填欄位")]
		[Display(Name = "密碼")]
		public string Password { get; set; }
		[Required(ErrorMessage = "帳號為必填欄位")]
		[StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "名字需介於3個字到30個字之間")]
		[Display(Name = "我的名字")]
		public string Name { get; set; }
		[Display(Name = "性別")]
		public string Gender { get; set; }
		[Display(Name = "我的生日")]//Readonly
		public DateTimeOffset? BirthDate { get; set; }
		[EmailAddress(ErrorMessage = "電子郵件格式錯誤")]
		[Display(Name = "我的郵件")]
		public string Email { get; set; }
		[Display(Name = "我的電話")]
		public string Phone { get; set; }
		[Display(Name = "身分")]
		public string Role { get; set; }
		[Display(Name = "註冊日期")]
		public DateTimeOffset RegisterDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
