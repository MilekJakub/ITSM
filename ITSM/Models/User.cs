#pragma warning disable CS8618



using Microsoft.AspNetCore.Identity;

namespace ITSM.Models
{
    public class User : IdentityUser
    {
        public string Forename { get; set; }
        public string Surname { get; set; }

        public int? JobPositionId { get; set; }
        public JobPosition? JobPosition { get; set; }

        public IEnumerable<WorkItem> WorkItems { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
