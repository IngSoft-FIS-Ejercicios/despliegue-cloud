using Domain;

namespace DomainTests;

[TestClass]
public class NotificationTest
{
    private User _user;
    private Comment _comment;
    private Notification _notification;

    [TestInitialize]
    public void SetUp()
    {
        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            Type = TypeUser.User,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        
        _comment = new Comment()
        {
            Id = 1,
            Resolved = false,
            Description = "Comment description",
            ResolverId = _user.Id,
            DateResolution = new DateTime(2024, 10, 26)
        };
        
        _notification = new Notification()
        {
            Id = 1,
            Message = "message"

        };
    }
    
    [TestMethod]
    public void CreateNotificationOk()
    {
        
        Assert.IsNotNull(_notification);
        Assert.AreEqual("message", _notification.Message);
        Assert.AreEqual(1, _notification.Id);
        
    }
    
}