using System.ComponentModel.DataAnnotations;

namespace Shocker_Project.Models
{
    public class ClientCasesMetaData
    {
        [Display(Name = "問題類別")]
        public int QuestionCategoryId { get; set; }
        [Display(Name = "問題描述")]
        [StringLength(300,ErrorMessage="最長不超過300個字")]
        public string ?Description { get; set; }
        [Display(Name="狀態")]
        public string? Status { get; set; }
        [Display(Name ="時間")]
        public DateTime? CloseDate { get; set; }
        [Display(Name ="回應")]
        public string ?Reply { get; set; }


    }
}