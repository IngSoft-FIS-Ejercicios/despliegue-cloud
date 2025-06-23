using Domain;
using IRepository;

namespace Logic;

public class EpicLogic
{
    private IRepositoryFull<Epic> _repoEpic;
    
    public EpicLogic(IRepositoryFull<Epic> repoEpic)
    {
        _repoEpic = repoEpic;
    }
    
    public void CreateEpic(Epic epic)
    {
        _repoEpic.Add(epic);
    }
    
    public void RemoveEpic(Epic epic)
    {
        _repoEpic.Remove(epic);
    }
    
    public void ModifyEpic(Epic toModify, Epic modified)
    {
        _repoEpic.Modify(toModify, modified);
    }
    
    public Epic FindEpicById(int? id)
    {
        return _repoEpic.FindById(id);
    }
    
    public bool Exists(Epic epic)
    {
        return _repoEpic.Exists(epic);
    }
    
    public List<Epic> GetAllEpics()
    {
        return _repoEpic.GetAll();
    }
}