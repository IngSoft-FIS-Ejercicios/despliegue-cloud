namespace IRepository;

public interface IRepository<T>
{
    void Add(T entity);
    void Modify(T entity, T modifiedEntity);
    T FindById(int? id);
    List<T> GetAll();
    
}