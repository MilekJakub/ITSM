#pragma warning disable CS8618

using ITSM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITSM.ViewModels
{
    public class HomeKanbanViewModel
    {
        public Project? Project { get; set; }
        public IEnumerable<State> States { get; set; }
        public IEnumerable<WorkItem> WorkItems { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}
