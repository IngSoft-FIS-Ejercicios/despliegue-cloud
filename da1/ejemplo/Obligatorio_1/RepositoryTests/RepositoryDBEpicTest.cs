using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBEpicTest
{
    private RepositoryDBEpic _repositoryDbEpic;
    private Epic _epic;
    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryDbEpic = new RepositoryDBEpic(_context);
        
        _epic = new Epic(new DateTimeProvider())
        {
            Id = 1,
            Title = "Epic 1",
            Description = "This is an epic",
            Priority = "Medium",
            DueDate = new DateTime(2025,12,31)
        };
    }

    [TestMethod]
    public void AddValidEpic()
    {
        _repositoryDbEpic.Add(_epic);
        Assert.IsTrue(_context.Epics.Contains(_epic));
    }

    [TestMethod]
    public void AddNullEpicError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryDbEpic.Add(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
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
        _repositoryDbEpic.Add(_epic);
        _repositoryDbEpic.Modify(_epic, newEpic);
        var modifiedEpic = _repositoryDbEpic.FindById(_epic.Id);
        Assert.AreEqual(newEpic.Description, modifiedEpic.Description);
        Assert.AreEqual(newEpic.Title, modifiedEpic.Title);

    }

    [TestMethod]
    public void FindEpicByIdOk()
    {
        _repositoryDbEpic.Add(_epic);
        var foundEpic = _repositoryDbEpic.FindById(_epic.Id);
        Assert.IsNotNull(foundEpic);
    }

    [TestMethod]
    public void FindEpicError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryDbEpic.FindById(1);
        }, "Epic not found");
        Assert.AreEqual("Epic not found", exc.Message);
    }
    
    [TestMethod]
    public void GetAllEpicsOk()
    {
        _repositoryDbEpic.Add(_epic);
        var epics = _repositoryDbEpic.GetAll();
        CollectionAssert.AreEqual(_context.Epics.ToList(), epics);
    }
    
    [TestMethod]
    public void RemoveEpicOk()
    {
        _repositoryDbEpic.Add(_epic);
        _repositoryDbEpic.Remove(_epic);
        Assert.IsTrue(_context.Epics.Count() == 0);
    }

    [TestMethod]
    public void RemoveEpicError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryDbEpic.Remove(_epic);
        }, "Epic not found");
        Assert.AreEqual("Epic not found", exc.Message);
    }

    [TestMethod]
    public void ExistsEpicOk()
    {
        _repositoryDbEpic.Add(_epic);
        Assert.IsTrue(_repositoryDbEpic.Exists(_epic));
    }
}