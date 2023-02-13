#pragma warning disable CS8618

using ITSM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Data
{
    public class BoardsContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<State> States { get; set; }

        public BoardsContext(DbContextOptions<BoardsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder =>
            {
                builder
                    .HasOne(x => x.JobPosition)
                    .WithMany(y => y.Users)
                    .HasForeignKey(x => x.JobPositionId);

                builder
                    .HasMany(x => x.WorkItems)
                    .WithOne(y => y.User)
                    .HasForeignKey(y => y.UserId);

                builder
                    .HasMany(x => x.Comments)
                    .WithOne(y => y.User)
                    .HasForeignKey(y => y.UserId);
            });

            modelBuilder.Entity<WorkItem>(builder =>
            {
                builder
                    .Property(x => x.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                builder
                    .Property(x => x.Description)
                    .IsRequired();

                builder
                    .Property(x => x.Priority)
                    .HasColumnType("TINYINT")
                    .IsRequired();

                builder
                    .HasOne(x => x.State)
                    .WithMany(y => y.WorkItems)
                    .HasForeignKey(x => x.StateId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder
                    .HasOne(x => x.Project)
                    .WithMany(y => y.WorkItems)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasMany(x => x.Comments)
                    .WithOne(y => y.WorkItem)
                    .HasForeignKey(y => y.WorkItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder
                    .HasMany(x => x.Tags)
                    .WithMany(y => y.WorkItems)
                    .UsingEntity<WorkItemTags>(builder =>
                    {
                        builder
                            .HasOne(x => x.WorkItem)
                            .WithMany()
                            .HasForeignKey(x => x.WorkItemId)
                            .OnDelete(DeleteBehavior.NoAction);

                        builder
                            .HasOne(x => x.Tag)
                            .WithMany()
                            .HasForeignKey(x => x.TagId)
                            .OnDelete(DeleteBehavior.NoAction);

                        builder
                            .HasKey(x => new { x.WorkItemId, x.TagId });
                    });
            });

            modelBuilder.Entity<WorkItemTags>(builder =>
            {
                builder
                    .HasKey(x => new { x.WorkItemId, x.TagId });
            });

            modelBuilder.Entity<Issue>(builder =>
            {
                builder
                    .Property(x => x.Effort)
                    .HasColumnType("TINYINT");
            });

            modelBuilder.Entity<Models.Task>(builder =>
            {
                builder
                    .Property(x => x.Activity)
                    .HasMaxLength(50);

                builder
                    .Property(x => x.RemainingWork)
                    .HasColumnType("TINYINT");
            });

            modelBuilder.Entity<Epic>(builder =>
            {
                builder
                    .Property(x => x.StartDate)
                    .IsRequired();

                builder
                    .Property(x => x.EndDate)
                    .IsRequired();
            });

            modelBuilder.Entity<Comment>(builder =>
            {
                builder
                    .Property(x => x.CreationDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                builder
                    .Property(x => x.UpdateDate)
                    .ValueGeneratedOnUpdate();

                builder
                    .Property(x => x.Message)
                    .IsRequired();

                builder
                    .HasOne(x => x.WorkItem)
                    .WithMany(y => y.Comments)
                    .HasForeignKey(x => x.WorkItemId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(x => x.Project)
                    .WithMany(y => y.Comments)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<JobPosition>(builder =>
            {
                builder
                    .Property(x => x.Position)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Project>(builder =>
            {
                builder
                    .Property(x => x.Name)
                    .HasMaxLength(50);

                builder
                    .HasMany(x => x.Comments)
                    .WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder
                    .HasMany(x => x.WorkItems)
                    .WithOne(x => x.Project)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<State>(builder =>
            {
                builder
                    .Property(x => x.Name)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Tag>(builder =>
            {
                builder
                    .Property(x => x.Value)
                    .HasMaxLength(25);
            });
        }
    }
}
