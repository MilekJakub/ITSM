using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}