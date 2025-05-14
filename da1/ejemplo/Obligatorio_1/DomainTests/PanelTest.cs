using Domain;

namespace DomainTests;

[TestClass]

public class PanelTest
{

    private Panel _panel;
    private Team _team;


    [TestInitialize]
    public void SetUp()
    {
        User user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        _team = new Team();
        _panel = new Panel()
        {
            Id = 1,
            Name = "DefaultPanel",
            Creator = user,
            CreatorId = user.Id,
            Team = _team,
            Description = "Default description", 
            PanelTaskList = new List<PanelTask> { }
        };
    }

    [TestMethod]
    
    public void CreatePanelOk()
    {
        
        Assert.AreEqual(1, _panel.Id);
        Assert.AreEqual("DefaultPanel", _panel.Name);
        Assert.AreEqual("Lucas", _panel.Creator.Name);
        Assert.AreEqual(_team, _panel.Team);
        Assert.AreEqual("Default description", _panel.Description);
        Assert.AreEqual(0, _panel.PanelTaskList.Count);
        Assert.AreEqual(_panel.CreatorId, 1);
    }

    [TestMethod]
    public void CreatePanelEmptyNameError()
    {

        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _panel.Name = "";
        }, "Name cannot be null or empty");

        Assert.AreEqual("Name cannot be null or empty", exc.Message);

    }

    [TestMethod]
    public void CreatePanelEmptyCreatorError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _panel.Creator = null;
        }, "Creator cannot be null");
        Assert.AreEqual("Creator cannot be null", exc.Message);
    }

    [TestMethod]
    public void AddTaskOk()
    {
        PanelTask task = new PanelTask(new DateTimeProvider());
        _panel.addTask(task);
        Assert.AreEqual(1, _panel.PanelTaskList.Count);
    }

    [TestMethod]
    public void AddNullTaskError()
    {
        PanelTask task = null;
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _panel.addTask(task);
        }, "Task cannot be null");
        Assert.AreEqual("Task cannot be null", exc.Message);
    }

    [TestMethod]

    public void RemoveTaskOk()
    {
        PanelTask task = new PanelTask(new DateTimeProvider());
        _panel.addTask(task);
        _panel.removeTask(task);
        Assert.AreEqual(0, _panel.PanelTaskList.Count);
    }
    
}
    