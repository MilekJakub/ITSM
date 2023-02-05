#pragma warning disable CS8618

namespace ITSM.Models
{
    public abstract class WorkItem
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }
    }

    public class Epic : WorkItem
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        public decimal Effort { get; set; }
    }

    public class Task : WorkItem
    {
        public string Activity { get; set; }
        public decimal RemainingWork { get; set; }
    }
}