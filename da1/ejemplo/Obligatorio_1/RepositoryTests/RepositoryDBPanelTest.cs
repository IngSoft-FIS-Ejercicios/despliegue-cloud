using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBPanelTest
{
    private RepositoryDBPanel _repositoryPanel;
    private SqlContext _context;
    private Panel _panel;
    private Team _team;
    private User _user;
    
    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryPanel = new RepositoryDBPanel(_context);

        _user = new User()
        {
            Id = 1,
            Name = "DefaultUser",
            Email = "default@gmail.com",
            Password = "1234asdFF#",
            Type = TypeUser.Admin
        };
        _team = new Team()
        {
            Id = 1,
            TeamName = "DefaultTeam",
            Administrator = _user,
            TeamUsersList = { _user }
        };
        
        _panel = new Panel()
        {
            Id = 1,
            Name = "DefaultPanel",
            Creator = _user,
            Team = _team,
            Description = "Default description", 
        };
    }
    
    [TestMethod]
    public void AddPanelOk()
    {
        _repositoryPanel.Add(_panel);
        Assert.IsTrue(_context.Panels.Contains(_panel));
    }

    [TestMethod]
    public void AddPanelNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryPanel.Add(null);
        }, "Panel cannot be null");
        
        Assert.AreEqual("Panel cannot be null", exc.Message);
    }
    
    [TestMethod]
    public void RemovePanelOk()
    {
        _repositoryPanel.Add(_panel);
        _repositoryPanel.Remove(_panel);
        Assert.IsTrue(_context.Panels.Count() == 0);
    }

    [TestMethod]
    public void RemoveNonExistentPanelError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryPanel.Remove(_panel);
        }, "Panel not found");
        Assert.AreEqual("Panel not found", exc.Message);
        
    }

    [TestMethod]
    public void ModifyPanelOk()
    {
        Panel panel2 = new Panel()
        {
            Id = 2,
            Name = "Panel2",
            Creator = _user,
            Team = _team,
            Description = "Description2",
        };
        
        _repositoryPanel.Add(_panel);
        _repositoryPanel.Modify(_panel, panel2);
        var modifiedPanel = _repositoryPanel.FindById(1);
        Assert.AreEqual(modifiedPanel.Name, panel2.Name);
        Assert.AreEqual(modifiedPanel.Description, panel2.Description);
        
    }
    
    [TestMethod]
    public void FindByIdOk()
    {
        _repositoryPanel.Add(_panel);
        var panel = _repositoryPanel.FindById(1);
        Assert.IsNotNull(panel);
    }

    [TestMethod]
    public void FindByIdError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryPanel.FindById(1);
        }, "Panel not found");
        Assert.AreEqual("Panel not found", exc.Message);
    }
    
    [TestMethod]
    public void GetAllOk()
    {
        _repositoryPanel.Add(_panel);
        Assert.IsTrue(_repositoryPanel.GetAll().Count == 1);
    }
    
    [TestMethod]
    public void ExistsOk()
    {
        _repositoryPanel.Add(_panel);
        Assert.IsTrue(_repositoryPanel.Exists(_panel));
    }
}