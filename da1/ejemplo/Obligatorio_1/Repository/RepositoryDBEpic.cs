using Domain;
using IRepository;

namespace Repository;

public class RepositoryDBEpic : IRepositoryFull<Epic>
{
    private readonly SqlContext _database;
    
    public RepositoryDBEpic(SqlContext database)
    {
        _database = database;
    }
    public void Add(Epic entity)
    {
        if(entity  == null) throw new ArgumentException("Epic cannot be null");
        _database.Epics.Add(entity);
        _database.SaveChanges();
    }

    public void Modify(Epic entity, Epic modifiedEntity)
    {
        var toModify = FindById(entity.Id);
        toModify.Title = modifiedEntity.Title;
        toModify.Description = modifiedEntity.Description;
        _database.Update(toModify);
        _database.SaveChanges();
    }

    public Epic FindById(int? id)
    {
        Epic findedEpic = _database.Epics.FirstOrDefault(e => e.Id == id);
        
        if(findedEpic == null) throw new ArgumentException("Epic not found");
        
        return findedEpic;
    }

    public List<Epic> GetAll()
    {
        return _database.Epics.ToList();
    }

    public void Remove(Epic entity)
    {
        var toRemove = FindById(entity.Id);
        _database.Epics.Remove(toRemove);
        _database.SaveChanges();
    }

    public bool Exists(Epic entity)
    {
        return _database.Epics.Contains(entity);
    }
    
}