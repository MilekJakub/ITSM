using ITSM.Models;
using ITSM.Services;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [Authorize]
    public class JobPositionController : Controller
    {
        private readonly IJobPositionService _jobPositionService;

        public JobPositionController(IJobPositionService jobPositionService)
        {
            _jobPositionService = jobPositionService;
        }

        public IActionResult Index()
        {
            var jobPositions = _jobPositionService.GetAll();

            var jobPositionVM = new PositionViewModel()
            {
                Position = new JobPosition(),
                JobPositions = jobPositions
            };

            return View(jobPositionVM);
        }

        [HttpPost]
        public IActionResult Index(PositionViewModel jobPositionVM)
        {
            var result = _jobPositionService.AddJobPosition(jobPositionVM.Position);

            if(result) TempData["success"] = $"{jobPositionVM.Position.Position} has been added!";
            else TempData["error"] = "Something went wrong, please try again.";

            return RedirectToAction(nameof(Index));
        }
    }
}
