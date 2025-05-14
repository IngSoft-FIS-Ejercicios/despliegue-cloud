using Domain;

namespace IRepository;

public interface IRepositoryPreFull<T>
{
    void Add(T entity);
    
    void Remove(T entity);

    List<T> GetData(User entity);
}