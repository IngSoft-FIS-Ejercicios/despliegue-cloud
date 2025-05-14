using Domain;
using IRepository;

namespace Logic;

public class TrashpaperLogic
{
    private IRepositoryFull<PanelTask> _repoTask;
    private IRepositoryTrash<Trashpaper> _repositoryTrashpaper;
    
    public TrashpaperLogic(IRepositoryTrash<Trashpaper> repositoryTrashpaper, IRepositoryFull<PanelTask> repoTask)
    {
        _repositoryTrashpaper = repositoryTrashpaper;
        _repoTask = repoTask;
    }

    
    public void Add(Trashpaper entity)
    {
        _repositoryTrashpaper.Add(entity);
    }
    
    public void MoveToTrashpaper(Trashpaper entity, PanelTask task)
    {
        Trashpaper trashpaper = _repositoryTrashpaper.GetTrashpaper(entity.Id);
        if (trashpaper.ListTasks.Count >= 10)
        {
            task.commentList.Clear();
            _repoTask.Remove(task);
            throw new ArgumentException("Trashpaper is full, task will be removed permanently");
        }
        task.UserTrashpaperId = entity.UserId;
        _repositoryTrashpaper.AddTask(entity, task);
    }
    
    public void RecoverFromTrashpaper(Trashpaper entity, PanelTask task)
    {
        task.UserTrashpaperId = null;
        _repositoryTrashpaper.RemoveTask(entity, task);
    }
    
    public void RemoveFromTrashpaper(Trashpaper entity, PanelTask task)
    {
        _repositoryTrashpaper.RemoveTask(entity, task);
        _repoTask.Remove(task);
    }
    
    public Trashpaper GetTrashpaper(int trashId)
    {
        return _repositoryTrashpaper.GetTrashpaper(trashId);
    }
    
    public List<PanelTask> GetTasks(User entity)
    {
        return _repositoryTrashpaper.GetTasks(entity);
    }
}