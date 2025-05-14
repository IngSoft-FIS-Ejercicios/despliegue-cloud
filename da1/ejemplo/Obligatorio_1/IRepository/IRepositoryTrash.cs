using Domain;

namespace IRepository;

public interface IRepositoryTrash<T>
{
    void Add(T entity);
    void AddTask(T entity, PanelTask task);
    void RemoveTask(T entity, PanelTask task);
    
    Trashpaper GetTrashpaper(int userId);
    List<PanelTask> GetTasks(User entity);
}