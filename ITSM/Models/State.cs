#pragma warning disable CS8618

namespace ITSM.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<WorkItem> WorkItems { get; set; }
    }
}