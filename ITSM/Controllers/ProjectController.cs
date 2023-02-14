using ITSM.Data;
using ITSM.Models;
using ITSM.Services;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var truncatedProjects = _projectService.GetAll();

            return View(truncatedProjects);
        }

        [HttpPost]
        public IActionResult Create(ProjectDetailsViewModel projectDetailsVM)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), projectDetailsVM);

            var result = _projectService.Create(projectDetailsVM);

            if (result) TempData["success"] = "Project has been created successfully.";
            else TempData["error"] = "Something went wrong while creating a project.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projectDetailsVM = _projectService.GetProjectDetailsVM(id);

            if (projectDetailsVM == null)
                return RedirectToAction(nameof(Index));

            return View(projectDetailsVM);
        }

        [HttpPost]
        public IActionResult Update(ProjectDetailsViewModel projectDetailsVM, int id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { projectDetailsVM.Project.Id });

            var result = _projectService.Update(projectDetailsVM);

            if (result) TempData["success"] = "Project has been updated succesfully.";
            else TempData["error"] = "Something went wrong while updating the project.";

            return RedirectToAction(nameof(Details), new { projectDetailsVM.Project.Id });
        }

        public IActionResult Delete(int id)
        {
            var result = _projectService.Delete(id);

            if (result) TempData["success"] = "Project has been deleted succesfully.";
            else TempData["error"] = "Something went wrong while deleting the project.";

            return RedirectToAction(nameof(Index));
        }
    }
}
