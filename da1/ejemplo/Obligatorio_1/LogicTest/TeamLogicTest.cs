using Repository;
using Domain;
using Logic;
using RepositoryTests.Context;

namespace LogicTest;

[TestClass]
public class TeamLogicTest
{
    private RepositoryDBTeam _repositoryTeam;
    private RepositoryDBPanel _repositoryPanel;
    private RepositoryDBPanelTask _repositoryTask;
    private TeamLogic _teamLogic;
    private Team _team;
    private Team _newTeam;
    private User _user;
    private User _admin;
    private User _newUser;
    private Panel _panel;
    private SqlContext _context;
    PanelLogic _panelLogic;

    [TestInitialize]

    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryTeam = new RepositoryDBTeam(_context);
        _repositoryPanel = new RepositoryDBPanel(_context);
        _repositoryTask = new RepositoryDBPanelTask(_context);
        _panelLogic = new PanelLogic(_repositoryPanel, _repositoryTask);
        _teamLogic = new TeamLogic(_repositoryTeam);

        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        _newUser = new User()
        {
            Id = 3,
            Name = "Manuel",
            Surname = "Lopez",
            Email = "manuel@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Manuel305#"
        };
        _admin = new User()
        {
            Id = 2,
            Name = "Pepe",
            Surname = "Gonzalez",
            Email = "pepe@gmail.com",
            Type = TypeUser.Admin,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Pepeee20#"
        };
        
        _team = new Team()
        {
            TeamName = "Team Name",
            TeamCreationDate = "07/12/2001",
            TaskDescription = "Task Description",
            MaxUsersAllowed = 20,
            Administrator = _user,
            TeamUsersList = new List<User>(){_user}
        };
        _newTeam = new Team()
        {
            TeamName = "New Name",
            TeamCreationDate = _team.TeamCreationDate,
            TaskDescription = "New description",
            MaxUsersAllowed = 25,
            Administrator = _team.Administrator,
            TeamUsersList = _team.TeamUsersList
        };
        
        _panel = new Panel()
        {
            Id = 2,
            Name = "Outdated Tasks",
            Creator = _admin,
            Team = _team,
            Description = "Panel that contains outdated tasks",
        };
    }
    
    [TestMethod]
    public void CreateTeamOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Assert.IsTrue(_teamLogic.GetTeams().Contains(_team));
    }

    [TestMethod]
    public void CreateTeamError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _teamLogic.CreateTeam(_user, _team);
        }, "User cannot create teams");
        Assert.AreEqual("User cannot create teams", exc.Message);
    }
    
    [TestMethod]
    public void RemoveTeamOk()
    {
        _teamLogic.CreateTeam(_admin, _team);

        _teamLogic.RemoveTeam(_admin, _team, _panelLogic);
        Assert.IsFalse(_teamLogic.GetTeams().Contains(_team));
    }

    [TestMethod]
    public void RemoveTeamError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
;
            _teamLogic.RemoveTeam(_user, _team,_panelLogic);
        }, "User cannot remove teams");
        Assert.AreEqual("User cannot remove teams", exc.Message);
    }
    
    
    [TestMethod]
    public void AddUserToTeamOk()
    {
        Team teamAdded = _teamLogic.CreateTeam(_admin, _team);
        _context.Users.Add(_newUser);
        _teamLogic.AddMember(teamAdded.Id, _admin,_newUser);
        Assert.IsTrue(teamAdded.TeamUsersList.Contains(_newUser));
    }
    
    [TestMethod]
    public void AddUserToTeamError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.Administrator = _admin;
            _teamLogic.AddMember(_team.Id, _user, _admin);
        }, "User cannot add members to team");
        Assert.AreEqual("User cannot add members to team", exc.Message);
    }
    
    [TestMethod]
    public void AddRepeatedUserToTeamError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _teamLogic.AddMember(_team.Id, _admin,_user);
            _teamLogic.AddMember(_team.Id, _admin,_user);
        }, "User already in team");
        Assert.AreEqual("User already in team", exc.Message);
    }
    
    [TestMethod]
    public void RemoveUserFromTeamOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        _context.Users.Add(_newUser);
        _teamLogic.AddMember(_team.Id, _admin, _newUser);
        _teamLogic.RemoveMember(_team,_admin, _newUser);
        Assert.IsFalse(_team.TeamUsersList.Contains(_newUser));
    }
    
    [TestMethod]
    public void RemoveUserFromTeamError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.Administrator = _admin;
            _teamLogic.RemoveMember(_team, _user, _user);
        }, "User cannot remove members from team");
        Assert.AreEqual("User cannot remove members from team", exc.Message);
    }

    [TestMethod]
    public void RemoveUserNonExistentError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        _context.Users.Add(_newUser);
        _teamLogic.AddMember(_team.Id, _admin, _newUser);
        _teamLogic.RemoveMember(_team, _user, _newUser);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _teamLogic.RemoveMember(_team, _user, _newUser);
        }, "User not in team");
        Assert.AreEqual("User not in team", exc.Message);
    }
    
    [TestMethod]
    public void RemoveAdminError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _teamLogic.RemoveMember(_team, _admin, _user);
        }, "Administrator cannot be removed from team");
        Assert.AreEqual("Administrator cannot be removed from team", exc.Message);
    }
    
    [TestMethod]
    public void ModifyTeamOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        _teamLogic.ModifyTeam(_admin, _team, _newTeam);
        Assert.AreEqual(_team.TeamName, _newTeam.TeamName);
        Assert.AreEqual(_team.TaskDescription, _newTeam.TaskDescription);
        Assert.AreEqual(_team.MaxUsersAllowed, _newTeam.MaxUsersAllowed);
    }

    [TestMethod]
    public void ModifyTeamError()
    {
        _teamLogic.CreateTeam(_admin, _team);
        _team.Administrator = _admin;

        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _teamLogic.ModifyTeam(_user, _team, _newTeam);
        }, "User cannot modify team");
        Assert.AreEqual("User cannot modify team", exc.Message);
        
    }
    [TestMethod]
    public void RemoveAdminWithActivesPanelError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _teamLogic.CreateTeam(_admin, _team);
            _panelLogic.CreatePanel(_panel);
            _teamLogic.RemoveTeam(_admin, _team, _panelLogic);
        }, "You cannot remove a team with active panels");
        Assert.AreEqual("You cannot remove a team with active panels", exc.Message);
    }

    [TestMethod]
    public void FindTeamOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Assert.AreSame(_team, _teamLogic.FindTeamById(_team.Id));
    }

    [TestMethod]
    public void CheckUserIsAdminOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Assert.IsTrue(_teamLogic.CheckUserIsAdmin(_user));
    }
    
    [TestMethod]
    public void CheckUserIsNotAdminOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        _team.Administrator = _admin;
        Assert.IsFalse( _teamLogic.CheckUserIsAdmin(_user));
    }
    
    [TestMethod]
    public void GetTeamsOk()
    {
        _teamLogic.CreateTeam(_admin, _team);
        Assert.AreEqual(1, _teamLogic.GetTeams().Count);
    }
}