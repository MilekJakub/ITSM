#pragma warning disable CS8618

using ITSM.Models;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Data
{
    public class BoardsContext : DbContext
    {
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }

        public BoardsContext(DbContextOptions<BoardsContext> options) : base(options) { }
    }
}
