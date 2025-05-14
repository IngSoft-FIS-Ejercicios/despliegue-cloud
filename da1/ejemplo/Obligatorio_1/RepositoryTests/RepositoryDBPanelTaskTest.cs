using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBPanelTaskTest
{
    private RepositoryDBPanelTask _repositoryDbPanelTask;
    private PanelTask _panelTask;
    private PanelTask _panelTask2;
    private SqlContext _context;
    
    [TestInitialize]

    public void Setup()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryDbPanelTask = new RepositoryDBPanelTask(_context);


        _panelTask = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025, 12, 12),
            commentList = new List<Comment>()
        };
        _panelTask2 = new PanelTask(new DateTimeProvider())
        {
            Id = 2,
            Title = "Task 2",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025, 12, 12),
            commentList = new List<Comment>()
        };
        
        
    }
    
    [TestMethod]
    public void AddPanelTaskOk()
    {
        _repositoryDbPanelTask.Add(_panelTask);
        Assert.IsTrue( _context.Tasks.Contains(_panelTask));
    }
    
    [TestMethod]
    public void AddPanelTaskNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryDbPanelTask.Add(null);
        },"PanelTask cannot be null");
        Assert.AreEqual("PanelTask cannot be null", exc.Message);
    }
    
    [TestMethod]
    public void RemovePanelTaskOk()
    {
        _repositoryDbPanelTask.Add(_panelTask);
        _repositoryDbPanelTask.Remove(_panelTask);
        Assert.IsTrue(_context.Tasks.Count() == 0);
    }
    
    [TestMethod]
    public void RemoveNonExistentPanelTaskError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {   
            _repositoryDbPanelTask.Remove(_panelTask);
        }, "PanelTask not found");
        Assert.AreEqual("PanelTask not found", exc.Message);
        
    }

    [TestMethod]
    public void UpdatePanelTaskOk()
    {
        _repositoryDbPanelTask.Add(_panelTask);
        _repositoryDbPanelTask.Modify(_panelTask,_panelTask2);
        var modified = _repositoryDbPanelTask.FindById(_panelTask.Id);
        Assert.AreEqual( modified.Title,_panelTask2.Title);
        Assert.AreEqual(modified.Description,_panelTask2.Description);
    }
    
    
    [TestMethod]
    public void FindPanelTaskByIdOk()
    {
        _repositoryDbPanelTask.Add(_panelTask);
        var foundTask = _repositoryDbPanelTask.FindById(1);
        Assert.IsNotNull(foundTask);
    }

    [TestMethod]
    public void FindPanelTaskError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryDbPanelTask.FindById(1);
        }, "PanelTask not found");
        Assert.AreEqual("PanelTask not found", exc.Message);
    }
    
    [TestMethod]
    public void GetAllPanelTaskOk()
    {
        _repositoryDbPanelTask.Add(_panelTask);
        _repositoryDbPanelTask.Add(_panelTask2);
        Assert.AreEqual(2,_repositoryDbPanelTask.GetAll().Count);
    }
    
    [TestMethod]
    public void ExistsPanelTaskOk()
    {
        _repositoryDbPanelTask.Add(_panelTask);
        Assert.IsTrue(_repositoryDbPanelTask.Exists(_panelTask));
    }
    
}