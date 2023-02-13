using ITSM.Models;
using ITSM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [Authorize]
    public class StateController : Controller
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        public IActionResult Index()
        {
            var truncatedServices = _stateService.GetAll();

            return View(truncatedServices);
        }

        [HttpPost]
        public IActionResult Create(State state)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), state);

            var result = _stateService.Create(state);

            if (result) TempData["success"] = "Item state has been created successfully.";
            else TempData["error"] = "Something went wrong while creating an item state.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0)
                return View(new State());

            var state = _stateService.GetById(id);

            if (state == null)
                return RedirectToAction(nameof(Index));

            return View(state);
        }

        [HttpPost]
        public IActionResult Update(State state, int id)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), state);

            var result = _stateService.Update(state);

            if (result) TempData["success"] = "Item state has been updated successfully.";
            else TempData["error"] = "Something went wrong while updating an item state.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var result = _stateService.Delete(id);

            if (result) TempData["success"] = "Item state has been deleted successfully.";
            else TempData["error"] = "Something went wrong while deleting an item state.";

            return RedirectToAction(nameof(Index));
        }
    }
}
