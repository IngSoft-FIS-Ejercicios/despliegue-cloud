using Domain;
using IRepository;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryDBPanelTask : IRepositoryFull<PanelTask>
{
    
    private SqlContext _database;
    
    public RepositoryDBPanelTask(SqlContext database)
    {
        _database = database;
    }
    public void Add(PanelTask entity)
    {
        if(entity == null) throw new ArgumentException("PanelTask cannot be null");
        _database.Tasks.Add(entity);
        _database.SaveChanges();
    }

    public void Modify(PanelTask entity, PanelTask modifiedEntity)
    {
        var toModify = FindById(entity.Id);
        toModify.Title = modifiedEntity.Title;
        toModify.Description = modifiedEntity.Description;
        toModify.Epic = modifiedEntity.Epic;
        toModify.DueDate = modifiedEntity.DueDate;
        toModify.State = modifiedEntity.State;
        toModify.EstimatedTime = modifiedEntity.EstimatedTime;
        toModify.InvestedTime = modifiedEntity.InvestedTime;
        _database.Update(toModify);
        _database.SaveChanges();
    }

    public PanelTask FindById(int? id)
    {
        PanelTask findedPanelTask = _database.Tasks
            .Include(t => t.commentList)
            .Include(t => t.Epic)
            .FirstOrDefault(e => e.Id == id);
        
        if(findedPanelTask == null) throw new ArgumentException("PanelTask not found");
        
        return findedPanelTask;
    }

    public List<PanelTask> GetAll()
    {
        return _database.Tasks.ToList();
    }

    public void Remove(PanelTask entity)
    {
        var toRemove = FindById(entity.Id);
        _database.Tasks.Remove(toRemove);
        _database.SaveChanges();
    }

    public bool Exists(PanelTask entity)
    {
        return _database.Tasks.Contains(entity);
    }
}