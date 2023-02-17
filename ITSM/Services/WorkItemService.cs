using ITSM.Data;
using ITSM.Models;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public interface IWorkItemService
    {
        IEnumerable<WorkItem> GetAll();
        WorkItemDetailsViewModel? GetViewModel(int id, string? discriminator);
        bool Create(WorkItemDetailsViewModel workItemVM);
        bool Update(WorkItemDetailsViewModel workItemVM);
        bool Delete(int id);
        bool ChangeWorkItemState(int workItemId, int stateId);
    }

    public class WorkItemService : IWorkItemService
    {
        private readonly BoardsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkItemService(BoardsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IEnumerable<WorkItem> GetAll()
        {
            var workItems = _dbContext.WorkItems
                .Include(x => x.Project)
                .Include(x => x.State)
                .Include(x => x.User)
                .ToList();

            return workItems;
        }

        public WorkItemDetailsViewModel? GetViewModel(int id, string? discriminator)
        {
            if (id == 0 && discriminator != null)
            {
                var newWorkItemVM = new WorkItemDetailsViewModel()
                {
                    ItemStates = GetStatesAsSelectItems(),
                    Projects = GetProjectsAsSelectItems(),
                    Users = GetUsersAsSelectItems(),
                    Discriminator = discriminator
                };

                return newWorkItemVM;
            }
            else
            {
                var workItem = _dbContext.WorkItems.FirstOrDefault(x => x.Id == id);

                if (workItem == null)
                    return null;

                var workItemVM = new WorkItemDetailsViewModel()
                {
                    ItemStates = GetStatesAsSelectItems(),
                    Projects = GetProjectsAsSelectItems(),
                    Users = GetUsersAsSelectItems()
                };

                workItemVM.Id = workItem.Id;
                workItemVM.Title = workItem.Title;
                workItemVM.Description = workItem.Description;
                workItemVM.Priority = workItem.Priority;

                workItemVM.StateId = workItem.StateId;
                workItemVM.ProjectId = workItem.ProjectId;
                workItemVM.UserId = workItem.UserId;

                workItemVM.Discriminator = workItem.Discriminator;

                switch (workItem.Discriminator)
                {
                    case "Task":
                        var task = (Models.Task)workItem;
                        workItemVM.Activity = task.Activity;
                        workItemVM.RemainingWork = task.RemainingWork;
                        break;

                    case "Issue":
                        var issue = (Issue)workItem;
                        workItemVM.Effort = issue.Effort;
                        break;

                    case "Epic":
                        var epic = (Epic)workItem;
                        workItemVM.StartDate = epic.StartDate;
                        workItemVM.EndDate = epic.EndDate;
                        break;

                    default:
                        return null;
                }

                return workItemVM;
            }
        }

        public bool Create(WorkItemDetailsViewModel workItemVM)
        {
            try
            {
                switch (workItemVM.Discriminator)
                {
                    case "Task":
                        var task = new Models.Task()
                        {
                            Id = workItemVM.Id,
                            Title = workItemVM.Title,
                            Description = workItemVM.Description,
                            Priority = workItemVM.Priority,
                            StateId = workItemVM.StateId,
                            ProjectId = workItemVM.ProjectId,
                            UserId = workItemVM.UserId,

                            Activity = workItemVM.Activity!.Value,
                            RemainingWork = workItemVM.RemainingWork!.Value
                        };
                        _dbContext.Tasks.Add(task);
                        break;

                    case "Issue":
                        var issue = new Issue()
                        {
                            Id = workItemVM.Id,
                            Title = workItemVM.Title,
                            Description = workItemVM.Description,
                            Priority = workItemVM.Priority,
                            StateId = workItemVM.StateId,
                            ProjectId = workItemVM.ProjectId,
                            UserId = workItemVM.UserId,

                            Effort = workItemVM.Effort!.Value
                        };
                        _dbContext.Issues.Add(issue);
                        break;

                    case "Epic":
                        var epic = new Epic()
                        {
                            Id = workItemVM.Id,
                            Title = workItemVM.Title,
                            Description = workItemVM.Description,
                            Priority = workItemVM.Priority,
                            StateId = workItemVM.StateId,
                            ProjectId = workItemVM.ProjectId,
                            UserId = workItemVM.UserId,

                            StartDate = workItemVM.StartDate!.Value,
                            EndDate = workItemVM.EndDate!.Value
                        };

                        _dbContext.Epics.Add(epic);
                        break;

                    default:
                        return false;
                }

                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool Update(WorkItemDetailsViewModel workItemVM)
        {
            var dbWorkItem = _dbContext.WorkItems.FirstOrDefault(x => x.Id == workItemVM.Id);

            if (dbWorkItem == null)
                return false;

            try
            {
                dbWorkItem.Title = workItemVM.Title;
                dbWorkItem.Description = workItemVM.Description;
                dbWorkItem.Priority = workItemVM.Priority;
                dbWorkItem.StateId = workItemVM.StateId;
                dbWorkItem.ProjectId = workItemVM.ProjectId;
                dbWorkItem.UserId = workItemVM.UserId;

                switch (workItemVM.Discriminator)
                {
                    case "Task":
                        var dbTask = (Models.Task)dbWorkItem;
                        dbTask.Activity = workItemVM.Activity!.Value;
                        dbTask.RemainingWork = workItemVM.RemainingWork!.Value;
                        break;

                    case "Issue":
                        var dbIssue = (Issue)dbWorkItem;
                        dbIssue.Effort = workItemVM.Effort!.Value;
                        break;

                    case "Epic":
                        var dbEpic = (Epic)dbWorkItem;
                        dbEpic.StartDate = workItemVM.StartDate!.Value;
                        dbEpic.EndDate = workItemVM.EndDate!.Value;
                        break;

                    default:
                        return false;
                }

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
            var workItem = _dbContext.WorkItems.FirstOrDefault(x => x.Id == id);

            if (workItem == null)
                return false;

            try
            {
                _dbContext.Remove(workItem);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private IEnumerable<SelectListItem> GetStatesAsSelectItems()
        {
            var states = _dbContext.States.ToList();
            var statesAsSelectItems = new List<SelectListItem>();

            foreach (var state in states)
            {
                var listItem = new SelectListItem()
                {
                    Text = state.Name,
                    Value = state.Id.ToString()
                };

                statesAsSelectItems.Add(listItem);
            }

            return statesAsSelectItems;
        }

        public bool ChangeWorkItemState(int workItemId, int stateId)
        {
            var workItem = _dbContext.WorkItems.FirstOrDefault(x => x.Id == workItemId);

            if (workItem == null)
                return false;

            if (stateId == 0)
            {
                workItem.StateId = null;
                _dbContext.SaveChanges();

                return true;
            }
            
            var state = _dbContext.States.FirstOrDefault(x => x.Id == stateId);

            if(state == null)
                return false;

            workItem.StateId = stateId;
            _dbContext.SaveChanges();

            return true;
        }

        private IEnumerable<SelectListItem> GetProjectsAsSelectItems()
        {
            var projects = _dbContext.Projects.ToList();
            var projectsAsSelectItems = new List<SelectListItem>();

            foreach (var project in projects)
            {
                var listItem = new SelectListItem()
                {
                    Text = project.Name,
                    Value = project.Id.ToString()
                };

                projectsAsSelectItems.Add(listItem);
            }

            return projectsAsSelectItems;
        }

        private IEnumerable<SelectListItem> GetUsersAsSelectItems()
        {
            var users = _userManager.Users.ToList();
            var usersAsSelectItems = new List<SelectListItem>();

            foreach (var user in users)
            {
                var listItem = new SelectListItem()
                {
                    Text = user.Email,
                    Value = user.Id.ToString()
                };

                usersAsSelectItems.Add(listItem);
            }

            return usersAsSelectItems;
        }
    }
}
