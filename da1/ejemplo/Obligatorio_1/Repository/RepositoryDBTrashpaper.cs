using Domain;
using IRepository;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryDBTrashpaper : IRepositoryTrash<Trashpaper>
{
    private SqlContext _database;
    
    public RepositoryDBTrashpaper(SqlContext context)
    {
        _database = context;
    }
    public void Add(Trashpaper entity)
    {
        _database.Trashpapers.Add(entity);
        _database.SaveChanges();
    }

    public void AddTask(Trashpaper entity, PanelTask task)
    {
        Trashpaper trashpaper = _database.Trashpapers.FirstOrDefault(x => x.Id == entity.Id);
        trashpaper.AddTask(task);
        _database.SaveChanges();
    }

    public void RemoveTask(Trashpaper entity, PanelTask task)
    {
        Trashpaper trashpaper = _database.Trashpapers.FirstOrDefault(x => x.Id == entity.Id);
        trashpaper.RemoveTask(task);
        _database.SaveChanges();
    }

    public List<PanelTask> GetTasks(User entity)
    {
        Trashpaper trashpaper = _database.Trashpapers
            .Include(t => t.ListTasks)
            .FirstOrDefault(x => x.UserId == entity.Id);
        return trashpaper?.ListTasks ?? new List<PanelTask>();
    }
    
    public Trashpaper GetTrashpaper(int userId)
    {
        return _database.Trashpapers.FirstOrDefault(x => x.UserId == userId);
    }
}