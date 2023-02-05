#pragma warning disable CS8618

namespace ITSM.Models
{
    public class WorkItemTags
    {
        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
