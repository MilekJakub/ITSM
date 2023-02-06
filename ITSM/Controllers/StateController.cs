using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    public class StateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
