#pragma warning disable CS8618

namespace ITSM.Models
{
    public class JobPosition
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}