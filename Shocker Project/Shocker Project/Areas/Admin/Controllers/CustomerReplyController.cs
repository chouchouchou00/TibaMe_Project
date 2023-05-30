using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Areas.Admin.Models;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerReplyController : Controller
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public CustomerReplyController(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        // GET: Admin/CustomerReply
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetQA()
        {
            var clientQA = _context.ClientCases;
                
            return Json(clientQA);
        }
        [HttpPost]
        public async Task<JsonResult> ReplyQA([FromBody] ClientCaseViewModels ccvm)
        {
            if (ModelState.IsValid)
            {
                ClientCases cc = await _context.ClientCases.FindAsync(ccvm.CaseId);
                if (cc == null)
                {
                    return Json(new { Message = "案件不存在" });
                }
                cc.AdminAccount = ccvm.AdminAccount;
                if (cc.Status == "cc0")
                {
                    cc.Status = "cc1";
                }
                else
                {
                    return Json(new { Message = "已結案" });
                }
                cc.Reply = ccvm.Reply;
                cc.CloseDate = DateTime.Now;
                _context.Update(cc);
                await _context.SaveChangesAsync();
                return Json(new { Message = "回復完畢" });
            }
            else
            {
                return Json(new { Message = "格式錯誤" });
            };
        }
        [HttpPost("Filter")]
        public async Task<JsonResult> FilterQA([FromBody] ClientCaseFilterViewModels ccf) 
        {
            return Json(_context.ClientCases.Where(c =>
                c.CaseId == ccf.CaseId ||
                c.UserAccount.Contains(ccf.UserAccount) ||
                c.Status.Contains(ccf.Status) ||
                c.QuestionCategory.CategoryName.Contains(ccf.QuestionCategory.CategoryName)
                ).Select(c => new ClientCases
                {
                    CaseId = c.CaseId,
                    UserAccount = c.UserAccount,
                    Status = c.Status,
                    QuestionCategory = c.QuestionCategory,
                }));
        }
    }
}

   
