using Domain;
using IRepository;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryDBPanel : IRepositoryFull<Panel>
{
    private readonly SqlContext _database;
    
    public RepositoryDBPanel(SqlContext database)
    {
        _database = database;
    }
    public void Add(Panel entity)
    {
        if(entity == null) throw new ArgumentException("Panel cannot be null");
        _database.Panels.Add(entity);
        _database.SaveChanges();
    }

    public void Modify(Panel entity, Panel modifiedEntity)
    {
        var toModify = FindById(entity.Id);
        toModify.Name = modifiedEntity.Name;
        toModify.Description = modifiedEntity.Description;
        _database.Update(toModify);
        _database.SaveChanges();
    }

    public Panel FindById(int? id)
    {
        Panel findedPanel = _database.Panels
            .Include(t => t.PanelTaskList)
            .Include(t => t.Team)
            .FirstOrDefault(p => p.Id == id);
        
        if(findedPanel == null) throw new ArgumentException("Panel not found");
        
        return findedPanel;
    }

    public List<Panel> GetAll()
    {
        return _database.Panels
            .Include(p => p.Team)
            .ThenInclude(u => u.TeamUsersList)
            .Include(p => p.PanelTaskList) 
            .ThenInclude(t => t.commentList) 
            .ToList();
    }

    public void Remove(Panel entity)
    {
        var toRemove = FindById(entity.Id);
        _database.Panels.Remove(toRemove);
        _database.SaveChanges();
    }

    public bool Exists(Panel entity)
    {
        return _database.Panels.Contains(entity);
    }
    
}