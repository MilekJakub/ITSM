using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    public class WorkItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
