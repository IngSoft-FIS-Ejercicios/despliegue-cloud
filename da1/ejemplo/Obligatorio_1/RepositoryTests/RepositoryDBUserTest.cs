using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBUserTest
{
    private RepositoryDBUser _repositoryUser;
    private User _user;
    private User _user2;
    private SqlContext context;

    [TestInitialize]
    public void SetUp()
    {
        context = SqlContextFactory.CreateMemoryContext();
        context.Database.EnsureDeleted();

        _repositoryUser = new RepositoryDBUser(context);
        
        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        
        _user2 = new User()
        {
            Id = 2,
            Name = "Marcelo",
            Surname = "Gonzalez",
            Email = "marcelo2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Marcelo19#"
        };
        
    }
    
    [TestMethod]
    public void AddUserOk()
    {
        _repositoryUser.Add(_user);
        Assert.AreEqual(1, context.Users.Count());
    }
    
    [TestMethod]
    public void AddUserNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryUser.Add(null);
        }, "User cannot be null");
        
        Assert.AreEqual("User cannot be null", exc.Message);
    }
    
    [TestMethod]
    public void RemoveUserOk()
    {
        _repositoryUser.Add(_user);
        _repositoryUser.Remove(_user);
        Assert.AreEqual(0,context.Users.Count());
    }

    [TestMethod]
    public void RemoveNonExistentUserError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryUser.Remove(_user);
        }, "User not found");
        Assert.AreEqual("User not found", exc.Message);
        
    }

    [TestMethod]
    public void ModifyUser()
    {
        _repositoryUser.Add(_user);
        _repositoryUser.Modify(_user, _user2);
        User findedUser = _repositoryUser.FindById(_user.Id);
        Assert.AreEqual(findedUser.Name, _user2.Name);
    }

    [TestMethod]
    public void FindByEmailOk()
    {
        _repositoryUser.Add(_user);
        _repositoryUser.FindByEmail("lucas2004@gmail.com");
        User findedUser = _repositoryUser.FindById(_user.Id);
        Assert.AreEqual(findedUser.Email, _user.Email);
    }
    
    [TestMethod]
    public void FindByEmailError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryUser.FindByEmail("lucas2004@gmail.com");
        }, "There is no user with that email");
        Assert.AreEqual("There is no user with that email", exc.Message);
    }

    [TestMethod]
    public void EmailExistTrue()
    {
        _repositoryUser.Add(_user);
        Assert.IsTrue(_repositoryUser.EmailExists(_user.Email));
    }
    
    [TestMethod]
    public void EmailExistFalse()
    {
        Assert.IsFalse(_repositoryUser.EmailExists(_user.Email));
    }
    
    [TestMethod]
    public void GetAllOk()
    {
        _repositoryUser.Add(_user);
        Assert.AreEqual(1, _repositoryUser.GetAll().Count());
    }
    
    [TestMethod]
    public void ExistsOk()
    {
        _repositoryUser.Add(_user);
        Assert.IsTrue(_repositoryUser.Exists(_user));
    }
    
    
    
}