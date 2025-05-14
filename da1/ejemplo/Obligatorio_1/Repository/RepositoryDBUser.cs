using Domain;
using IRepository;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryDBUser : IRepositoryUser<User>
{
    private SqlContext _database;

    public RepositoryDBUser(SqlContext context)
    {
        _database = context;
    }

    public void Add(User userEntity)
    {
        if (userEntity is null)
        {
            throw new ArgumentException("User cannot be null");

        }
        _database.Add(userEntity);
        _database.SaveChanges();
    }

    public void Modify(User entity, User modifiedEntity)
    {
        User toModify = FindById(entity.Id);
        toModify.Name = modifiedEntity.Name;
        toModify.Surname = modifiedEntity.Surname;
        toModify.BirthDate = modifiedEntity.BirthDate;
        toModify.Email = modifiedEntity.Email;
        toModify.Password = modifiedEntity.Password;
        toModify.Type = modifiedEntity.Type;
        _database.Update(toModify);
        _database.SaveChanges();
    }

    public User FindById(int? id)
    {
        User user = _database.Users
            .FirstOrDefault(x => x.Id == id);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        return user;
    }

    public List<User> GetAll()
    {
        return _database.Users.ToList();
    }

    public void Remove(User entity)
    {
        User toRemove = FindById(entity.Id);
        _database.Users.Remove(toRemove);
        _database.SaveChanges();
    }

    public bool Exists(User entity)
    {
        return _database.Users.Contains(entity);
    }

    public User FindByEmail(string email)
    {
        User user = _database.Users
            .FirstOrDefault(x => x.Email == email);
        if (user is null)
        {
            throw new ArgumentException("There is no user with that email");
        }

        return user;
    }

    public bool EmailExists(string email)
    {
        User user = _database.Users.FirstOrDefault(x => x.Email == email);
        return user != null;
    }
    
}