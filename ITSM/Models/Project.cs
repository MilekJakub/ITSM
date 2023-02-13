#pragma warning disable CS8618

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ITSM.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ValidateNever]
        public IEnumerable<WorkItem> WorkItems { get; set; }
        [ValidateNever]
        public IEnumerable<Comment> Comments { get; set; }
    }
}