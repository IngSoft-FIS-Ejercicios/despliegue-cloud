using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityConfigurations;

public static class PrechargedData
{
    public static void Precharge(ModelBuilder modelBuilder)
    {
        // Precharged admin
        User admin = new User()
        {
            Id = 1,
            Name = "Admin",
            Surname = "Lopez",
            Email = "admin@gmail.com",
            Type = TypeUser.Admin,
            Password = "Admin20#",
            BirthDate = new DateTime(2024, 10, 20)
        };
        modelBuilder.Entity<User>().HasData(admin);
        // Precharged panel
        modelBuilder.Entity<Panel>().HasData(
            new Panel
            {
                Id = 1,
                Name = "Outdated Tasks",
                Description = "Tasks that are outdated",
                Team = null,
                CreatorId = admin.Id,
                PanelTaskList = {}
            });
        
        // Precharged trashpaper for admin
        modelBuilder.Entity<Trashpaper>().HasData(
            new Trashpaper
            {
                Id = 1,
                UserId = admin.Id
            });
        
        modelBuilder.Entity<Panel>()
            .HasOne(p => p.Creator)
            .WithMany() // 
            .HasForeignKey("CreatorId"); 
        
        modelBuilder.Entity<Panel>()
            .HasOne(p => p.Team)
            .WithMany() 
            .IsRequired(false); 

    }
}