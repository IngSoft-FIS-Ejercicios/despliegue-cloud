using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBTeamTest
{
    private RepositoryDBTeam _repositoryTeam;
    private SqlContext _context;
    private Team _team1;
    private Team _team2;
    private User _user;

    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryTeam = new RepositoryDBTeam(_context);

        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        _team1 = new Team()
        {
            Id = 1,
            TeamName = "Team Name",
            TeamCreationDate = "07/12/2001",
            TaskDescription = "Task Description",
            MaxUsersAllowed = 20,
            Administrator = _user
        };
        _team2 = new Team()
        {
            Id = 2,
            TeamName = "Team Name 2",
            TeamCreationDate = "08/12/2001",
            TaskDescription = "Task Description",
            MaxUsersAllowed = 10,
            Administrator = _user
        };
        
    }
    
    [TestMethod]
    public void CreateRepositoryTeamOk()
    {
        Assert.IsNotNull(_repositoryTeam);
    }
    
    [TestMethod]
    public void AddTeamOk()
    {
        _repositoryTeam.Add(_team1);
        Assert.IsTrue(_context.Teams.Contains(_team1));
        _repositoryTeam.Add(_team2);
        Assert.IsTrue(_context.Teams.Contains(_team2));
        Assert.AreEqual(_context.Teams.Count(), 2);
    }

    [TestMethod]
    public void AddTeamNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() => 
        {
            _repositoryTeam.Add(null);
        }, "Team cannot be null");
        Assert.AreEqual("Team cannot be null", exc.Message);
    }

    [TestMethod]

    public void RemoveTeamOk()
    {
        _repositoryTeam.Add(_team1);
        _repositoryTeam.Remove(_team1);
        Assert.IsTrue(_context.Teams.Count() == 0);
    }

    [TestMethod]

    public void RemoveNonExistentTeamError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryTeam.Remove(_team2);
        }, "Team not found");
        Assert.AreEqual("Team not found", exc.Message);
    }

    [TestMethod]
    public void ModifyTeamOk()
    {
        _repositoryTeam.Add(_team1);
        _repositoryTeam.Modify(_team1, _team2);
        var team = _repositoryTeam.FindById(1);
        Assert.AreEqual(team.TeamName, _team2.TeamName);
    }
    
    [TestMethod]
    public void GetAllTeamsOk()
    {
        _repositoryTeam.Add(_team1);
        _repositoryTeam.Add(_team2);
        Assert.AreEqual(2, _repositoryTeam.GetAll().Count());
    }
    
    
    [TestMethod]
    public void FindTeamOk()
    {
        _repositoryTeam.Add(_team1);
        var team = _repositoryTeam.FindById(1);
        Assert.IsNotNull(team);
    }


    
    [TestMethod]
    public void FindTeamError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryTeam.FindById(1);
        }, "Team not found");
        Assert.AreEqual("Team not found", exc.Message);
    }
    
    [TestMethod]
    public void ExistsTeamOk()
    {
        _repositoryTeam.Add(_team1);
        Assert.IsTrue(_repositoryTeam.Exists(_team1));
    }
    
}