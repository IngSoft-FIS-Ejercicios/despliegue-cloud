using Domain;
using Logic;
using Repository;
using RepositoryTests.Context;

namespace LogicTest;

[TestClass]
public class EpicLogicTest
{
    private RepositoryDBEpic _repositoryDbEpic;
    private EpicLogic _epicLogic;
    private SqlContext _context;
    private Epic _epic;
    
    [TestInitialize]
    public void TestInitialize()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryDbEpic = new RepositoryDBEpic(_context);
        
        _epicLogic = new EpicLogic(_repositoryDbEpic);
        
        _epic = new Epic(new DateTimeProvider())
        {
            Id = 1,
            Title = "Epic 1",
            Description = "This is an epic",
            Priority = "Medium",
            DueDate = new DateTime(2025, 12, 31)
        };
    }
    
    [TestMethod]
    public void AddValidEpic()
    {
        _epicLogic.CreateEpic(_epic);
        Assert.IsTrue(_repositoryDbEpic.Exists(_epic));
    }

    [TestMethod]
    public void RemoveEpicOk()
    {
        _epicLogic.CreateEpic(_epic);
        var epicToRemove = _context.Epics.Find(_epic.Id);
        _epicLogic.RemoveEpic(epicToRemove);
        Assert.IsFalse(_repositoryDbEpic.Exists(_epic));
    }
    
    [TestMethod]
    public void ModifyEpicOk()
    {
        var newEpic = new Epic(new DateTimeProvider())
        {
            Title = "Epic 1",
            Description = "modified epic",
            Priority = _epic.Priority,
            DueDate = _epic.DueDate
        };
        _epicLogic.CreateEpic(_epic);
        _epicLogic.ModifyEpic(_epic, newEpic);
        var modifiedEpic = _epicLogic.FindEpicById(_epic.Id);
        Assert.AreEqual(newEpic.Description, modifiedEpic.Description);
        Assert.AreEqual(newEpic.Title, modifiedEpic.Title);
    }
    
    [TestMethod]
    public void FindEpicByIdOk()
    {
        _epicLogic.CreateEpic(_epic);
        var foundEpic = _epicLogic.FindEpicById(_epic.Id);
        Assert.IsNotNull(foundEpic);
    }
    
    [TestMethod]
    public void ExistsEpicOk()
    {
        _epicLogic.CreateEpic(_epic);
        Assert.IsTrue(_epicLogic.Exists(_epic));
    }
    
    [TestMethod]
    public void GetAllEpicsOk()
    {
        var list = _epicLogic.GetAllEpics();
        Assert.IsNotNull(list);
    }
    
}