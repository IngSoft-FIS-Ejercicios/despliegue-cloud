using Repository;
using Domain;
using Logic;
using RepositoryTests.Context;

namespace LogicTest;

[TestClass]
public class UserLogicTest
{
    private UserLogic _userLogic;
    private RepositoryDBUser _repositoryUser;
    private RepositoryDBNotification _repositoryNotification;
    private User _user;
    private User _userModifier;
    private User _admin;
    private User _newUser;
    private User _noAdmin;
    private User _userOrAdmin;
    private SqlContext _context;

    [TestInitialize]

    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryUser = new RepositoryDBUser(_context);
        _repositoryNotification = new RepositoryDBNotification(_context);
        _userLogic = new UserLogic(_repositoryUser, _repositoryNotification);
        
        _user = new User()
        {
            Id = 2,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        _userModifier = new User()
        {
            Id = 5,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = _user.Email,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        
        _admin = new User()
        {
            Id = 1,
            Name = "Admin",
            Surname = "Lopez",
            Email = "admin@gmail.com",
            Type = TypeUser.Admin,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Admin20#"
        };
        
        
        _noAdmin = new User()
        {
            Id = 3,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            Type = TypeUser.User,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        
        _newUser = new User()
        {
            Id = 4,
            Name = "Andres",
            Surname = "Gonzalez",
            Email = "andres@gmail.com",
            Type = TypeUser.User,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        _userOrAdmin = new User()
        {
            Id = _user.Id,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "newLucasMAIL@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "newPass2000#",
            Type = TypeUser.Admin
        };
        
    }

    [TestMethod]
    public void AddUserOk()
    {
        _userLogic.AddUser(_admin, _user);
        Assert.IsTrue(_userLogic.Exists(_user));
    }

    [TestMethod]
    public void GetUserByIdOk()
    {
        _userLogic.AddUser(_admin, _user);
        Assert.AreEqual(_user, _userLogic.GetUserById(_user.Id));
    }

    [TestMethod]
    public void GetUserByIdError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.GetUserById(5);
        },"User not found");
        Assert.AreEqual("User not found", exc.Message);
    }
    

    [TestMethod]
    public void AddUserError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.AddUser(_noAdmin, _user);
        }, "Users cannot add other users");
        Assert.AreEqual("Users cannot add other users", exc.Message);
    }

    [TestMethod]
    public void AddUserRepeatedEmailError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.AddUser(_admin, _user);
            _userLogic.AddUser(_admin, _user);
        }, "Already exists an user with that email");
        Assert.AreEqual("Already exists an user with that email", exc.Message);
    }

    [TestMethod]
    public void RemoveUserOk()
    {
        _userLogic.AddUser(_admin, _user);
        _userLogic.RemoveUser(_admin, _user);
        Assert.IsFalse(_userLogic.Exists(_user));
    }

    [TestMethod]
    public void RemoveUserError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.RemoveUser(_noAdmin, _user);
        }, "Users cannot remove other users");
        Assert.AreEqual("Users cannot remove other users", exc.Message);
    }

    [TestMethod]
    public void RemoveUserError2()
    {
        
        _userLogic.AddUser(_admin, _user);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.RemoveUser(_admin, _admin);
        }, "User cannot remove himself");
        Assert.AreEqual("User cannot remove himself", exc.Message);
    }
    
    [TestMethod]
    public void ModifyUser()
    {
        _userLogic.AddUser(_admin, _user);
        _userLogic.ModifyUser(_user,_user, _userModifier);
        Assert.AreEqual(_userModifier.Name, _user.Name);
    }

    [TestMethod]
    public void ModifyUserChangePasswordError()
    {
        _userModifier.Password = "diferentPassword##20";
        Exception exc = Assert.ThrowsException<ArgumentException>(() => { _userLogic.ModifyUser(_user,_user, _userModifier); },
            "User cannot modify his password");
        Assert.AreEqual("User cannot modify his password", exc.Message);

    }
    
    [TestMethod]
    public void ModifyUserChanggeEmailError()
    {
        _userModifier.Email = "diferent@gmail.com";
        Exception exc = Assert.ThrowsException<ArgumentException>(() => { _userLogic.ModifyUser(_user,_user, _userModifier); },
            "User cannot modify his email");
        Assert.AreEqual("User cannot modify his email", exc.Message);

    }

    [TestMethod]
    public void ModifyAdmin()
    {
        _user.Type = TypeUser.Admin;
        _userLogic.AddUser(_admin, _user);
        _userLogic.ModifyUser(_user, _user, _userOrAdmin);
        Assert.AreEqual(_userOrAdmin.Email, _user.Email);
    }
    
    [TestMethod]
    public void LogInOk()
    {
        _userLogic.AddUser(_admin,_user);
        Assert.AreEqual(_user, _userLogic.LogIn(_user.Email, _user.Password));
    }

    [TestMethod]
    public void LogInIncorrectPasswordError()
    {
        _userLogic.AddUser(_admin,_user);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.LogIn(_user.Email, "Incorrect"); 
            
        },"Incorrect password");
        Assert.AreEqual("Incorrect password", exc.Message);
    }

    [TestMethod]
    public void LogInIncorrectEmailError()
    {
        _userLogic.AddUser(_admin,_user);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _userLogic.LogIn("Incorrect", _user.Password); 
            
        },"There is no user with that email");
        Assert.AreEqual("There is no user with that email", exc.Message);
    }
    [TestMethod]
    public void IsEmptyOk()
    {
        Assert.IsTrue(_userLogic.IsEmpty());
    }
    [TestMethod]
    public void GetAllOk()
    {
        _userLogic.AddUser(_admin, _user);
        Assert.AreEqual(_userLogic.GetAll().Count, 1);
    }
    
    [TestMethod]
    public void GetUserByEmailOk()
    {
        _userLogic.AddUser(_admin, _user);
        Assert.AreEqual(_user, _userLogic.GetUserByEmail(_user.Email));
    }

    [TestMethod]
    public void AddNotificationOk()
    {
        _userLogic.AddUser(_admin, _user);
        Notification not = new Notification()
        {
            UserId = _user.Id,
            Message = "This is a notification"
        };
        _userLogic.AddNotification(_user, not);
        Assert.AreEqual(_repositoryNotification.GetData(_user).Count(), 1);
    }
    
    [TestMethod]
    public void RemoveNotificationOk()
    {
        _userLogic.AddUser(_admin, _user);

        Notification notification = new Notification()
        {
            UserId = _user.Id,
            Message = "This is a notification"
        };
        _userLogic.AddNotification(_user, notification);
        _userLogic.RemoveNotification(_user, notification);
        Assert.AreEqual(_repositoryNotification.GetData(_user).Count(), 0);
    }
    
    [TestMethod]
    public void GetNotificationsOk()
    {
        _userLogic.AddUser(_admin, _user);
        Notification notification = new Notification()
        {
            UserId = _user.Id,
            Message = "This is a notification"
        };
        _userLogic.AddNotification(_user, notification);
        Assert.AreEqual(_userLogic.GetNotifications(_user).Count, 1);
    }
}