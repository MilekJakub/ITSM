using ITSM.Data;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Controllers
{
    public class ProjectController : Controller
    {
        private readonly BoardsContext _dbContext;

        public ProjectController(BoardsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var projects = 
                _dbContext.Projects
                .Include(x => x.WorkItems)
                .ToList();

            var projectVMs = new List<ProjectViewModel>();
            
            foreach (var project in projects)
            {
                var id = project.Id;
                var name = project.Name;
                var description = project.Description.Length > 100 ? TruncateString(project.Description, 100) : project.Description;
                var workItemsString = "";

                if (project.WorkItems != null && project.WorkItems.Any())
                {
                    var workItems = new List<string>();
                    foreach (var item in project.WorkItems)
                    {
                        workItems.Add(item.Title);
                    }

                    workItemsString = String.Join(", ", workItems);
                    workItemsString = workItemsString.Length > 50 ? TruncateString(workItemsString, 50) : workItemsString;
                }

                var projectVM = new ProjectViewModel()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    WorkItems = workItemsString
                };

                projectVMs.Add(projectVM);
            }

            return View(projectVMs);
        }

        private string TruncateString(string input, int Length)
        {
            input.Substring(0, Length - 3);
            input += "...";

            return input;
        }
    }
}
