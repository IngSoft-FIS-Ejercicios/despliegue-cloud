using Domain;

namespace IRepository;

public interface IRepositoryUser<T> : IRepositoryFull<T>
{
    T FindByEmail(string email);
    bool EmailExists(string email);
    
}