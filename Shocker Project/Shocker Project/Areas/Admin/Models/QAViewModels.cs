namespace Shocker_Project.Areas.Admin.Models
{
    public class QAViewModels
    {
        public int CaseId { get; set; }
        public int QuestionCategoryId { get; set; }
        public string ?UserAccount { get; set; }
        public string ?AdminAccount { get; set; }
        public string ?Description { get; set; }
        public string ?Status { get; set; }
        public DateTimeOffset? CloseDate { get; set; }
        public string ?Reply { get; set; }
    }
}
