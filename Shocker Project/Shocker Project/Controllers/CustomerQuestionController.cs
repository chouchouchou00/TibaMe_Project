using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shocker_Project.Models;


namespace Shocker_Project.Controllers
{
    public class CustomerQuestionController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public CustomerQuestionController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index( CustomerQAViewModel cqavm)
        {
            if (ModelState.IsValid)
            {
                cqavm.Status = "未回覆";
                cqavm.UserAccount = "User1";
                ClientCases clientCases = new ClientCases()
                {
                     Status = cqavm.Status,
                     UserAccount = cqavm.UserAccount,
                     Description = cqavm.Description,
                     QuestionCategoryId = cqavm.QuestionCategoryId,
                };
                _context.ClientCases.Add(clientCases);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}

