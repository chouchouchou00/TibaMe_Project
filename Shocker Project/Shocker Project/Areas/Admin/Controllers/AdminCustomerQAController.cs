using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Areas.Admin.Models;
using Shocker_Project.Models;
using System.Security.Claims;

namespace Shocker_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCustomerQAController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;
        public AdminCustomerQAController(db_a98a02_thm101team1001Context context) 
        {
           _context = context ;
        }

        public async Task <IActionResult> Index()
        {
            var QAT =_context.ClientCases.Include(p => p.Description);
            return View(await QAT.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index (QAViewModels qav) 
        {
            ClientCases cc = new ClientCases()
            {
                Status = qav.Status,
                UserAccount = qav.UserAccount,
                Description = qav.Description,
                QuestionCategoryId = qav.QuestionCategoryId,
            };
            return Ok();
        }
    }
}
