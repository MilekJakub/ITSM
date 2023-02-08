#pragma warning disable CS8618

using ITSM.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ITSM.ViewModels.ProjectViewModels
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }

        [ValidateNever]
        public IEnumerable<WorkItem>? UnassignedWorkItems { get; set; }
    }
}
