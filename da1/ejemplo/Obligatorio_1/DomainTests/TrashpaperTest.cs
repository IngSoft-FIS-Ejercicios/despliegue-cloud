using Domain;

namespace DomainTests;

[TestClass]
public class TrashpaperTest
{
    private Trashpaper _trashpaper;
    private PanelTask _task;
    private User _user;
    
    [TestInitialize]
    public void TestInitialize()
    {
        
        _task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task",
            Description = "Description",
            DueDate = DateTime.Now.AddDays(20),
        };
        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Password = "Admin20#",
            Surname = "Perez",
            Email = "lucas2004@gmail.com",
            Type = TypeUser.User,
        };
        _trashpaper = new Trashpaper()
        {
            Id = 1,
            UserId = _user.Id,
            ListTasks = new List<PanelTask>()
        };
        
    }

    [TestMethod]
    public void CreateTrashpaperOk()
    {
        Assert.AreEqual(1, _trashpaper.UserId);
        Assert.AreEqual(1, _trashpaper.Id);
        Assert.IsNotNull(_trashpaper.ListTasks);
    }
    
    [TestMethod]
    public void AddTaskOk()
    {
        _trashpaper.AddTask(_task);
        Assert.AreEqual(1, _trashpaper.ListTasks.Count);
    }
    
    [TestMethod]
    public void RemoveTaskOk()
    {
        _trashpaper.AddTask(_task);
        _trashpaper.RemoveTask(_task);
        Assert.AreEqual(0, _trashpaper.ListTasks.Count);
    }
}