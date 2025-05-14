namespace IRepository;

public interface IRepositoryFull<T> : IRepository<T>
{
    void Remove(T entity);
    bool Exists(T entity);
}