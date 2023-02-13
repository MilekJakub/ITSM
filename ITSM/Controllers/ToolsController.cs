using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    public class ToolsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
