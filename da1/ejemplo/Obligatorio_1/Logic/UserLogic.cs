using Domain;
using IRepository;

namespace Logic;

public class UserLogic
{
    private IRepositoryUser<User> _repoUser;

    private IRepositoryPreFull<Notification> _repositoryNotification;

    public UserLogic(IRepositoryUser<User> repoUser, IRepositoryPreFull<Notification> repositoryNotification)
    {
        _repoUser = repoUser;
        _repositoryNotification = repositoryNotification;
    }


    public User GetUserById(int? userId)
    {
        User user = _repoUser.FindById(userId);
        return user;
    }
    public void AddUser(User admin, User userOrAdmin)
    {
        if(admin.Type.ToString() == TypeUser.User.ToString())
        {
            throw new ArgumentException("Users cannot add other users");
        }
        
        bool repeatedEmail = _repoUser.EmailExists(userOrAdmin.Email);
        if (repeatedEmail)
        {
            throw new ArgumentException("Already exists an user with that email");
        }
        
        _repoUser.Add(userOrAdmin);
    }

    public void RemoveUser(User remover, User userOrAdmin)
    {
        if(remover.Type.ToString() == TypeUser.User.ToString())
        {
            throw new ArgumentException("Users cannot remove other users");
        }

        if (remover.Equals(userOrAdmin))
        {
            throw new ArgumentException("User cannot remove himself");
        }
        
        _repoUser.Remove(userOrAdmin);
    }

    public void ModifyUser(User modifier, User toModify, User modified)
    {
        if (modifier.Type.ToString() == TypeUser.User.ToString())
        {
            if (toModify.Password != modified.Password)
            {
                throw new ArgumentException("User cannot modify his password");
            }

            if (toModify.Email != modified.Email)
            {
                throw new ArgumentException("User cannot modify his email");
            }
        }
        
        _repoUser.Modify(toModify, modified);
    }
    
    public User LogIn(string email, string password)
    {
        User user = _repoUser.FindByEmail(email);

        if (user.Password != password)
        {
            throw new ArgumentException("Incorrect password");
        }
        return user;
    }
    

    public bool Exists(User user)
    {
        return _repoUser.Exists(user);
    }

    public bool IsEmpty()
    {
        return _repoUser.GetAll().Count() == 0;
    }

    public List<User> GetAll()
    {
        return _repoUser.GetAll();
    }
    
    public void AddNotification(User user, Notification notification)
    {
        _repositoryNotification.Add(notification);
    }
    
    public void RemoveNotification(User user, Notification notification)
    {
        _repositoryNotification.Remove(notification);
    }

    public List<Notification> GetNotifications(User user)
    {
        return _repositoryNotification.GetData(user);
    }
    
    public User GetUserByEmail(string email)
    {
        User user = _repoUser.FindByEmail(email);
        
        return user;
    }
}