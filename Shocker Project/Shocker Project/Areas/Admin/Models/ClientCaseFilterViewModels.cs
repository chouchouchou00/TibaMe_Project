using Shocker_Project.Models;

namespace Shocker_Project.Areas.Admin.Models
{
    public class ClientCaseFilterViewModels
    {
        public int CaseId { get; set; }
        public int QuestionCategoryId { get; set; }
        public string UserAccount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public virtual QuestionCategories QuestionCategory { get; set; }
        public virtual Users UserAccountNavigation { get; set; }
    }
}
