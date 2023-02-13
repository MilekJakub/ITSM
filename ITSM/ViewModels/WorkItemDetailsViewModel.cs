#pragma warning disable CS8618

using ITSM.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITSM.ViewModels
{
    public class WorkItemDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int? StateId { get; set; }
        public int? ProjectId { get; set; }
        public string? UserId { get; set; }

        [ValidateNever]
        public string Discriminator { get; set; }

        // Select lists
        [ValidateNever]
        public IEnumerable<SelectListItem> ItemStates { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Projects { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Users { get; set; }

        // Task
        public Activities? Activity { get; set; }
        public int? RemainingWork { get; set; }

        // Issue
        public int? Effort { get; set; }

        //Epic
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
