using Domain;
using IRepository;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBTrashpaperTest
{
    private RepositoryDBTrashpaper _repositoryDBTrashpaper;
    private SqlContext _context;
    private Trashpaper _trashpaper;
    
    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryDBTrashpaper = new RepositoryDBTrashpaper(_context);
        
        _trashpaper = new Trashpaper()
        {
            Id = 1,
            UserId = 1
        };
    }
    
    [TestMethod]
    public void AddTrashpaperOk()
    {
        _repositoryDBTrashpaper.Add(_trashpaper);
        Assert.AreEqual(1, _context.Trashpapers.Count());
    }
    
    [TestMethod]
    public void AddTaskOk()
    {
        PanelTask task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task",
            Description = "Description",
            DueDate = DateTime.Now.AddDays(20),
        };
        
        _repositoryDBTrashpaper.Add(_trashpaper);
        _context.Tasks.Add(task);
        _repositoryDBTrashpaper.AddTask(_trashpaper, task);
        Assert.AreEqual(1, _context.Trashpapers.FirstOrDefault(x => x.Id == _trashpaper.Id).ListTasks.Count);
    }
    
    [TestMethod]
    public void RemoveTaskOk()
    {
        PanelTask task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task",
            Description = "Description",
            DueDate = DateTime.Now.AddDays(20),
        };
        
        _repositoryDBTrashpaper.Add(_trashpaper);
        _context.Tasks.Add(task);
        _repositoryDBTrashpaper.AddTask(_trashpaper, task);
        _repositoryDBTrashpaper.RemoveTask(_trashpaper, task);
        Assert.AreEqual(0, _context.Trashpapers.FirstOrDefault(x => x.Id == _trashpaper.Id).ListTasks.Count);
    }
    
    [TestMethod]
    public void GetTasksOk()
    {
        PanelTask task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task",
            Description = "Description",
            DueDate = DateTime.Now.AddDays(20),
        };
        
        _repositoryDBTrashpaper.Add(_trashpaper);
        _context.Tasks.Add(task);
        _repositoryDBTrashpaper.AddTask(_trashpaper, task);
        User testUser = new User();
        testUser.Id = 1;

        Assert.AreEqual(1, _repositoryDBTrashpaper.GetTasks(testUser).Count);
    }
    
    [TestMethod]
    public void GetTrashpaperOk()
    {
        _repositoryDBTrashpaper.Add(_trashpaper);
        Assert.AreEqual(_trashpaper, _repositoryDBTrashpaper.GetTrashpaper(1));
    }
    
}