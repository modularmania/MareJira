using Microsoft.EntityFrameworkCore;

namespace MareJira.Objects;

public class TaskContext : DbContext {
    
    public DbSet<JiraTask> Tasks { get; set; }
    public DbSet<JiraUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JiraTask>(entity => {
            entity.HasKey(e => e.Name);
            entity.Property(e => e.Assignee).IsRequired();
            entity.Property(e => e.Deadline);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Priority).IsRequired();
            entity.Property(e => e.Progress).IsRequired();
        });
        
        modelBuilder.Entity<JiraUser>(entity => {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Tasks);
        });
    }
}