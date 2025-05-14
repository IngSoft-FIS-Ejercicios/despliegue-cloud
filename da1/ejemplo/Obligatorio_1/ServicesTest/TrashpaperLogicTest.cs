using Domain;
using Logic;
using Repository;
using RepositoryTests.Context;

namespace LogicTest;

[TestClass]
public class TrashpaperLogicTest
{
    private RepositoryDBTrashpaper _repositoryDBTrashpaper;
    private RepositoryDBPanelTask _repositoryDBPanelTask;
    private TrashpaperLogic _trashpaperLogic;
    private SqlContext _context;
    private Trashpaper _trashpaper;
    private PanelTask _task;
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryDBTrashpaper = new RepositoryDBTrashpaper(_context);
        _repositoryDBPanelTask = new RepositoryDBPanelTask(_context);
        _trashpaperLogic = new TrashpaperLogic(_repositoryDBTrashpaper, _repositoryDBPanelTask);
        _trashpaper = new Trashpaper()
        {
            Id = 1,
            UserId = 1
        };
        _task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Description = "Test",
            Title = "TestTitle",
        };
    }
    
    [TestMethod]
    public void AddTrashpaper()
    {
        Trashpaper trashpaper = new Trashpaper();
        
        _trashpaperLogic.Add(trashpaper);
        
        Assert.AreEqual(1, _context.Trashpapers.Count());
    }
    
    [TestMethod]
    public void MoveToTrashpaperOk()
    {
        _trashpaperLogic.Add(_trashpaper);
        _context.Tasks.Add(_task);
        _trashpaperLogic.MoveToTrashpaper(_trashpaper, _task);
        Assert.AreEqual(1, _context.Trashpapers.Find(_trashpaper.Id).ListTasks.Count);
    }
    
    [TestMethod]
    public void MoveToTrashpaperFull()
    {
        _trashpaperLogic.Add(_trashpaper);
        for (int i = 0; i < 10; i++)
        {
            PanelTask task = new PanelTask(new DateTimeProvider())
            {
                Id = i+10,
                Description = "Test",
                Title = "TestTitle",
            };
            _context.Tasks.Add(task);
            _trashpaperLogic.MoveToTrashpaper(_trashpaper, task);
        }

        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _context.Tasks.Add(_task);
            _context.SaveChanges();
            _trashpaperLogic.MoveToTrashpaper(_trashpaper, _task);
        }, "Trashpaper is full, task will be removed permanently");
        Assert.AreEqual("Trashpaper is full, task will be removed permanently", exc.Message);
    }
    
    [TestMethod]
    public void RemoveFromTrashpaperOk()
    {
        _trashpaperLogic.Add(_trashpaper);
        _context.Tasks.Add(_task);
        _trashpaperLogic.MoveToTrashpaper(_trashpaper, _task);
        _trashpaperLogic.RemoveFromTrashpaper(_trashpaper, _task);
        Assert.AreEqual(0, _context.Trashpapers.Find(_trashpaper.Id).ListTasks.Count);
    }
    
    [TestMethod]
    public void RecoverFromTrashpaperOk()
    {
        _trashpaperLogic.Add(_trashpaper);
        _context.Tasks.Add(_task);
        _trashpaperLogic.MoveToTrashpaper(_trashpaper, _task);
        _trashpaperLogic.RecoverFromTrashpaper(_trashpaper, _task);
        Assert.AreEqual(0, _context.Trashpapers.Find(_trashpaper.Id).ListTasks.Count);
    }
    
    [TestMethod]
    public void GetTasks()
    {
        _trashpaperLogic.Add(_trashpaper);
        _context.Tasks.Add(_task);
        _trashpaperLogic.MoveToTrashpaper(_trashpaper, _task);
        Assert.AreEqual(1, _trashpaperLogic.GetTasks(new User(){Id = 1}).Count);
    }
    
    [TestMethod]
    public void GetTrashpaper()
    {
        _trashpaperLogic.Add(_trashpaper);
        Assert.AreEqual(_trashpaper, _trashpaperLogic.GetTrashpaper(1));
    }
}