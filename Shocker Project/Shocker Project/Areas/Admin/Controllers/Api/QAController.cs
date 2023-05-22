using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Admin.Controllers.Api
{
    [Route("api/QA/{action}")]
    [ApiController]
    public class QAController : ControllerBase
    {
        private readonly db_a98a02_thm101team1001Context _db;

        public QAController(db_a98a02_thm101team1001Context db)
        {
            _db = db;
        }
        public object all() 
        {
           return _db.ClientCases.Select(x => new
            {
                AllQA = x,
               
            }).ToList() ;
        }
    }
}
