using ITSM.Data;
using ITSM.Models;
using ITSM.Services;
using ITSM.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Controllers
{
    public class ProjectController : Controller
    {
        private readonly BoardsContext _dbContext;
        private readonly IProjectService _projectService;

        public ProjectController(BoardsContext dbContext, IProjectService projectService)
        {
            _dbContext = dbContext;
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var truncatedProjects = _projectService.GetTruncatedProjects();

            return View(truncatedProjects);
        }

        [HttpPost]
        public IActionResult Create(ProjectDetailsViewModel projectDetailsVM)
        {
            if(!ModelState.IsValid)
                return View(projectDetailsVM);

            _projectService.Create(projectDetailsVM);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projectDetailsVM = _projectService.GetProjectDetailsVM(id);

            if(projectDetailsVM == null)
                return RedirectToAction(nameof(Index));

            return View(projectDetailsVM);
        }

        [HttpPost]
        public IActionResult Update(ProjectDetailsViewModel projectCreateVM, int id)
        {
            if(!ModelState.IsValid)
                return View(nameof(Details), projectCreateVM);

            _projectService.Update(projectCreateVM);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _projectService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
