using ITSM.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    public class StateController : Controller
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        public IActionResult Index()
        {
            var truncatedServices = _stateService.GetTruncatedStates();

            return View(truncatedServices);
        }
    }
}
