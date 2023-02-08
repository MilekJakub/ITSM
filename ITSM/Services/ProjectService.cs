using ITSM.Data;
using ITSM.Models;
using ITSM.ViewModels.ProjectViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public interface IProjectService
    {
        void Create(ProjectDetailsViewModel projectDetailsVM);
        Project? GetProjectById(int id);
        bool Update(ProjectDetailsViewModel projectCreateVM);
        bool Delete(int id);
        IEnumerable<TruncatedProjectViewModel> GetTruncatedProjects();
        ProjectDetailsViewModel? GetProjectDetailsVM(int projectId);
    }

    public class ProjectService : IProjectService
    {
        private readonly BoardsContext _dbContext;

        public ProjectService(BoardsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(ProjectDetailsViewModel projectDetailsVM)
        {
            _dbContext.Projects.Add(projectDetailsVM.Project);
            _dbContext.SaveChanges();
        }

        public Project? GetProjectById(int id)
        {
            var project = _dbContext.Projects.Include(x => x.WorkItems).FirstOrDefault(x => x.Id == id);

            return project;
        }

        public bool Update(ProjectDetailsViewModel projectCreateVM)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == projectCreateVM.Project.Id);

            if(project == null)
                return false;

            project.Name = projectCreateVM.Project.Name;
            project.Description = projectCreateVM.Project.Description;
            project.WorkItems = projectCreateVM.Project.WorkItems;

            _dbContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == id);

            if(project == null)
                return false;

            _dbContext.Remove(project);
            _dbContext.SaveChanges();

            return true;
        }

        public IEnumerable<TruncatedProjectViewModel> GetTruncatedProjects()
        {
            var projects =
                _dbContext.Projects
                .Include(x => x.WorkItems)
                .ToList();

            var truncatedProjects = new List<TruncatedProjectViewModel>();

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

                var truncatedProject = new TruncatedProjectViewModel()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    WorkItems = workItemsString
                };

                truncatedProjects.Add(truncatedProject);
            }

            return truncatedProjects;
        }

        public ProjectDetailsViewModel? GetProjectDetailsVM(int projectId)
        {
            var project = projectId == 0 ? new Project() : _dbContext.Projects.FirstOrDefault(x => x.Id == projectId);

            if (project == null)
                return null;

            var projectCreateVM = new ProjectDetailsViewModel()
            {
                Project = project,
                UnassignedWorkItems = _dbContext.WorkItems.Where(x => x.ProjectId == null).ToList()
            };

            return projectCreateVM;
        }

        private string TruncateString(string input, int length)
        {
            input = input.Substring(0, (length - 3));
            input += "...";

            return input;
        }
    }
}
