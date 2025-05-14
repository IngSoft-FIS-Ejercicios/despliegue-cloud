using Domain;
using Microsoft.EntityFrameworkCore;
using Repository.EntityConfigurations;

namespace Repository;

public class SqlContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Trashpaper> Trashpapers { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelTask> Tasks { get; set; }
    public DbSet<Epic> Epics { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if (!Database.IsInMemory()) this.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PrechargedData.Precharge(modelBuilder);
        modelBuilder.Entity<Panel>()
            .HasMany(p => p.PanelTaskList)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade); 


        modelBuilder.Entity<PanelTask>()
            .HasMany(t => t.commentList)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade); 
        
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification"); 
        });
        
        modelBuilder.Entity<PanelTask>()
            .HasOne<User>()
            .WithMany(u => u.Trashpaper)
            .HasForeignKey(task => task.UserTrashpaperId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Team>()
            .HasMany<User>(x => x.TeamUsersList)
            .WithMany(x => x.TeamsList);
        
        // Attribute team.Administrator has foreignt key to user id
        modelBuilder.Entity<Team>()
            .HasOne<User>(x => x.Administrator)
            .WithMany()
            .HasForeignKey(x => x.AdministratorId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
    
}