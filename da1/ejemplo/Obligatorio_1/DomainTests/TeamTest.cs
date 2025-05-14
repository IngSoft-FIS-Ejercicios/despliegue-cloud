using Domain;

namespace DomainTests;

[TestClass]
public class TeamTest
{
    private Team _team;
    private static User _admin = new User();
    
    [TestInitialize]
    public void SetUp()
    {   
        _team = new Team()
        {
            TeamName = "Team Name",
            TeamCreationDate = "07/12/2001",
            TaskDescription = "Task Description",
            MaxUsersAllowed = 20,
            Administrator = _admin,
            AdministratorId = _admin.Id,
            TeamUsersList = new List<User>()
            
        };
    }
    
    [TestMethod]
    public void CreateTeamOk()
    {
        
        Assert.IsNotNull(_team);
        Assert.AreEqual("Team Name",_team.TeamName);
        Assert.AreEqual(new DateTime(2001,12,7).ToString("dd/MM/yyyy"), _team.TeamCreationDate);
        Assert.AreEqual("Task Description", _team.TaskDescription);
        Assert.AreEqual(20, _team.MaxUsersAllowed);
        Assert.AreEqual(_admin, _team.Administrator);
        Assert.AreEqual(_admin.Id, _team.AdministratorId);

    }

    [TestMethod]
    public void CreateTeamEmptyNameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.TeamName = "";
        }, "TeamName cannot be empty");
        Assert.AreEqual("TeamName cannot be empty",exc.Message);
    }
    
    [TestMethod]
    public void CreateTeamNameOnlyWhitespaceError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.TeamName = "  ";
        }, "TeamName cannot be empty");
        Assert.AreEqual("TeamName cannot be empty",exc.Message);
    }
    
    [TestMethod]
    public void TeamNameTrimOk()
    {
        _team.TeamName = " Team 1 ";
        Assert.AreEqual("Team 1", _team.TeamName); 
    }

    [TestMethod]
    public void InvalidCreationDateError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.TeamCreationDate = "40/12/2001";
        }, "Date is invalid");
        Assert.AreEqual("Date is invalid",exc.Message);
        
    }
    
    [TestMethod]
    public void TaskDescriptionTrimOk()
    {
        _team.TaskDescription = " Task Description ";
        Assert.AreEqual("Task Description", _team.TaskDescription); 
    }

    [TestMethod]
    public void MaxUsersAllowedLowerThanOneError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.MaxUsersAllowed = 0;
        }, "MaxUsersAllowed must be greater than 0");
        Assert.AreEqual("MaxUsersAllowed must be greater than 0",exc.Message);
    }
    
    [TestMethod]
    public void MaxUsersAllowedSmallerThanActualTeamUserListSizeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            User user2 = new User();
            User user3 = new User();
            _team.addUser(user2);
            _team.addUser(user3);
            _team.MaxUsersAllowed = 1;
        }, "MaxUsersAllowed cant be lower than TeamUserList.Count, _teamUsersList.Count = " +_team.numberOfUsers());
        Assert.AreEqual("MaxUsersAllowed cant be lower than TeamUserList.Count, _teamUsersList.Count = " +_team.numberOfUsers(),exc.Message);
    }
    
    [TestMethod]
    public void MaxUsersAllowedNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.MaxUsersAllowed = null;
        }, "MaxUsersAllowed cannot be null");
        Assert.AreEqual("MaxUsersAllowed cannot be null",exc.Message);
    }

    [TestMethod]
    public void AdministratorNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _team.Administrator = null;
        }, "Administrator cannot be null");
        Assert.AreEqual("Administrator cannot be null",exc.Message);
    }
    
    [TestMethod]
    public void TeamUsersListAddOk()
    {
        User user2 = new User();
        _team.addUser(user2);
    }
    
    [TestMethod]
    public void TeamUsersListAddMoreThanAllowedError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            User user2 = new User();
            User user3 = new User();
            _team.addUser(user2);
            _team.MaxUsersAllowed = 1;
            _team.addUser(user3);
        }, "You cant have more users than allowed, MaxUsersAllowed = "+_team.MaxUsersAllowed);
        Assert.AreEqual("You cant have more users than allowed, MaxUsersAllowed = "+_team.MaxUsersAllowed,exc.Message);
    }
    
    
    
}