using ITSM.Data;
using ITSM.Services;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ITSM.Controllers
{
    public class HomeController : Controller
    {
        private readonly BoardsContext _dbContext;
        private readonly IWorkItemService _workItemService;

        public HomeController(BoardsContext dbContext, IWorkItemService workItemService)
        {
            _dbContext = dbContext;
            _workItemService = workItemService;
        }

        public IActionResult Index(int? projectId = null)
        {
            if (projectId == 0) projectId = null;

            var currentProject = _dbContext.Projects.FirstOrDefault(x => x.Id == projectId);
            var workItems = _dbContext.WorkItems.Include(x => x.State).Where(x => x.ProjectId == projectId);

            var itemStates = _dbContext.States.ToList();

            var projects = _dbContext.Projects.ToList();
            var projectList = new List<SelectListItem>();

            foreach (var project in projects)
            {
                var listItem = new SelectListItem()
                {
                    Text = project.Name,
                    Value = project.Id.ToString()
                };

                projectList.Add(listItem);
            }

            var kanbanVM = new HomeKanbanViewModel()
            {
                Project = currentProject,
                Projects = projectList,
                WorkItems = workItems,
                States = itemStates
            };

            return View(kanbanVM);
        }

        [HttpPost]
        public IActionResult Index(int projectId)
        {
            return RedirectToAction(nameof(Index), new { projectId = projectId });
        }

        public class Change
        {
            public string WorkItem { get; set; }
            public string ToState { get; set; }
        }

        public class KanbanRequest
        {
            public List<Change> Data { get; set; }
        }

        [HttpPost]
        public IActionResult ApproveChanges([FromBody] KanbanRequest body)
        {
            foreach (var item in body.Data)
            {
                int workItemId;
                int stateId = 0;

                if (!int.TryParse(item.WorkItem, NumberStyles.Integer, null, out workItemId))
                {
                    return BadRequest("Cannot parse work item id.");
                }

                if (item.ToState != null)
                {
                    if (!int.TryParse(item.ToState, NumberStyles.Integer, null, out stateId))
                    {
                        return BadRequest("Cannot parse work item id.");
                    }
                }
                
                var result = _workItemService.ChangeWorkItemState(workItemId, stateId);

                if (!result)
                    return BadRequest($"Cannot assign State with id: {stateId} to Work Item with id: {workItemId}.");
            }

            return Ok();
        }

    }
}