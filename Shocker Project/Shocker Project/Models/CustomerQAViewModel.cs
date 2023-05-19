using Microsoft.Build.Framework;
using Shocker_Project.Controllers;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Shocker_Project.Models
{
    public class CustomerQAViewModel
    {
   
        public int CaseId { get; set; }
        [Required(ErrorMessage = "請選擇類型")]
        [Display(Name = "請選擇問題類別")]
        public int QuestionCategoryId { get; set; }
        public string? UserAccount { get; set; }
        public string? AdminAccount { get; set; }
        [Required(ErrorMessage = "請輸入您遇到的問題")]
        [Display(Name = "請輸入問題")]
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset? CloseDate { get; set; }
        public string? Reply { get; set; }


    }
}
