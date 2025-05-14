using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBNotificationTest
{
    private RepositoryDBNotification _repositoryNotification;
    private SqlContext _context;
    private Notification _notification;

    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();

        _repositoryNotification = new RepositoryDBNotification(_context);

        _notification = new Notification()
        {
            Id = 1,
            Message = "Hello",
        };
    }
    
    [TestMethod]
    public void AddNotificationOk()
    {
        _repositoryNotification.Add(_notification);
        Assert.AreEqual(1, _context.Notifications.Count());
    }

    [TestMethod]
    public void RemoveNotificationOk()
    {
        _repositoryNotification.Add(_notification);
        _repositoryNotification.Remove(_notification);
        Assert.AreEqual(0, _context.Notifications.Count());
    }

    [TestMethod]
    public void GetNotificationsOk()
    {
        User user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Type = TypeUser.User,
            Password = "Lucas20#"

        };
        _notification.UserId = user.Id;
        _repositoryNotification.Add(_notification);
        Assert.AreEqual(1, _repositoryNotification.GetData(user).Count);
    }

}