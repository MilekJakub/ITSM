using ITSM.Data;
using ITSM.Models;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public interface IProjectService
    {
        bool Create(ProjectDetailsViewModel projectDetailsVM);
        Project? GetProjectById(int id);
        IEnumerable<Project>? GetAll();
        ProjectDetailsViewModel? GetProjectDetailsVM(int projectId);
        bool Update(ProjectDetailsViewModel projectCreateVM);
        bool Delete(int id);
    }

    public class ProjectService : IProjectService
    {
        private readonly BoardsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectService(BoardsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public bool Create(ProjectDetailsViewModel projectDetailsVM)
        {
            try
            {
                _dbContext.Projects.Add(projectDetailsVM.Project);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public Project? GetProjectById(int id)
        {
            var project = _dbContext.Projects
                .Include(x => x.WorkItems)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Id == id);

            return project;
        }

        public bool Update(ProjectDetailsViewModel projectCreateVM)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == projectCreateVM.Project.Id);

            if(project == null)
                return false;

            try
            {
                project.Name = projectCreateVM.Project.Name;
                project.Description = projectCreateVM.Project.Description;
                project.WorkItems = projectCreateVM.Project.WorkItems;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool Delete(int id)
        {
            var project = _dbContext.Projects.FirstOrDefault(x => x.Id == id);

            if(project == null)
                return false;

            try
            {
                _dbContext.Remove(project);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Project>? GetAll()
        {
            var projects =
                _dbContext.Projects
                .Include(x => x.WorkItems)
                .ToList();

            return projects;
        }

        public ProjectDetailsViewModel? GetProjectDetailsVM(int projectId)
        {
            var project = (projectId == 0) ? new Project() :
                _dbContext.Projects
                .Include(x => x.WorkItems)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Id == projectId);

            if (project == null)
                return null;

            var projectCreateVM = new ProjectDetailsViewModel()
            {
                Project = project,
                UnassignedWorkItems = _dbContext.WorkItems.Where(x => x.ProjectId == null).ToList(),
            };

            return projectCreateVM;
        }
    }
}
