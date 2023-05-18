using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;

namespace Shocker_Project.Controllers
{
    public class CustomerQuestion : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]

//        if(CustomerTA != string || CustomerTA != int )
//         {
//            alert("您輸入了非正確的字元")
//            return RedirectToAction(CustomerQuestion(Index))
//         }
//        else
//        {
//                _context.Add(QuestionCategories);
//                await _context.SaveChangesAsync();
//                alert("成功送出表單")
//                return RedirectToAction(nameof(Index));
//}
            
//        }
    }
}
