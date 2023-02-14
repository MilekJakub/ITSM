using ITSM.Models;
using ITSM.Services;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace ITSM.Controllers
{
    [Authorize]
    public class WorkItemController : Controller
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }

        public IActionResult Index()
        {
            var workItems = _workItemService.GetAll();

            return View(workItems);
        }

        [HttpGet]
        public IActionResult Details(int id, string? discriminator)
        {
            var workItemVM = _workItemService.GetViewModel(id, discriminator);

            if (workItemVM == null)
            {
                TempData["error"] = "Bad request.";
                return RedirectToAction(nameof(Index));
            }

            return View(workItemVM);
        }

        [HttpPost]
        public IActionResult Create(WorkItemDetailsViewModel workItemVM)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), workItemVM);

            var result = _workItemService.Create(workItemVM);

            if (result) TempData["success"] = "Task has been created successfully.";
            else TempData["error"] = "Something went wrong while creating your task.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Update(WorkItemDetailsViewModel workItemVM)
        {
            if (!ModelState.IsValid)
                return View(workItemVM);

            var result = _workItemService.Update(workItemVM);

            if (result) TempData["success"] = "Task has been updated successfully.";
            else TempData["error"] = "Something went wrong while updating your task.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _workItemService.Delete(id);

            if (result) TempData["success"] = "Task has been deleted successfully.";
            else TempData["error"] = "Something went wrong while deleting your task.";

            return RedirectToAction(nameof(Index));
        }
    }
}
