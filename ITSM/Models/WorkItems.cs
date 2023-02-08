#pragma warning disable CS8618

namespace ITSM.Models
{
    public abstract class WorkItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public string Discriminator { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }
        
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }

    public class Epic : WorkItem
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        public int Effort { get; set; }
    }

    public class Task : WorkItem
    {
        public string Activity { get; set; }

        public decimal RemainingWork { get; set; }
    }
}