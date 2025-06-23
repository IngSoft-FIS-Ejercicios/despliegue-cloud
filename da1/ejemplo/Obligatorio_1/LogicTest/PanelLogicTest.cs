using DocumentFormat.OpenXml.Spreadsheet;
using Domain;
using Repository;
using Logic;
using Moq;
using RepositoryTests.Context;
using Comment = Domain.Comment;

namespace LogicTest;

[TestClass]
public class PanelLogicTest
{
    private Panel _panel;
    private Panel _panel2;
    private Panel _panel3;
    private Panel _panelWithoutTeam;
    private PanelLogic _panelLogic;
    private User _user;
    private User _creator;
    private User _noCreator;
    private PanelTask _task1;
    private PanelTask _task2;
    private PanelTask _task3;
    private PanelTask _taskEpicless;
    private PanelTask _taskModifier;
    private PanelTask _expiredTask;
    private Comment _comment1;
    private Comment _comment2;
    private Team _team;
    private Epic _epic;
    private RepositoryDBPanel _repositoryPanel;
    private RepositoryDBPanelTask _repositoryPanelTask;
    private PanelTaskLogic _panelTaskLogic;
    private IDateTimeProvider _dataTimeProvider = new DateTimeProvider();
    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _user = new User()
        {
            Id = 95,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            Type = TypeUser.Admin,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        _team = new Team()
        {
            TeamName = "Team Name",
            TeamCreationDate = "07/12/2001",
            TaskDescription = "Task Description",
            MaxUsersAllowed = 20,
            Administrator = _user,
            AdministratorId = _user.Id,
            TeamUsersList = new List<User>(){_user}
            
        };
        _panel = new Panel()
        {
            Id = 2,
            Name = "DefaultPanel",
            Creator = _user,
            CreatorId = _user.Id,
            Team = _team,
            Description = "Default description", 
            PanelTaskList = new List<PanelTask> { }
        };
        
        _panel2 = new Panel()
        {
            Name = "New Name",
            Creator = new User(),
            Team = new Team(),
            Description = "New Description",
            PanelTaskList = new List<PanelTask> { }
        };

        _panel3 = new Panel()
        {
            Name = "Assist",
            Creator = new User(),
            Team = new Team(),
            Description = "New Description",
            PanelTaskList = new List<PanelTask> { }
        };

        _panelWithoutTeam = new Panel()
        {
            Id = _panel.Id,
            Name = "New Name",
            Creator = _panel.Creator,
            Description = "New Description",
            PanelTaskList = _panel.PanelTaskList
        };
        
        
        _creator = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        
        _noCreator = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };

        _epic = new Epic(new DateTimeProvider())
        {
            Id = 1,
            Title = "Epic 1",
            Priority = "Urgent",
            Description = "Study",
            DueDate = new DateTime(2025, 10, 20)
        };

        _task1 = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 5,
            InvestedTime = 2,
            Epic = _epic,
            commentList = new List<Comment>(),
        };

        _task2 = new PanelTask(new DateTimeProvider())
        {
            Id = 2,
            Title = "Task 2",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 10,
            InvestedTime = 6,
            Epic = _epic,
            commentList = new List<Comment>(),
            State = StateTask.Unfinished
        };
        
        _task3 = new PanelTask()
        {
            _dateTimeProvider = _dataTimeProvider,
            Id = 3,
            Title = "Task 2",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 10,
            InvestedTime = 20,
            Epic = _epic,
            commentList = new List<Comment>(),
            State = StateTask.Finished
        };

        _taskModifier = new PanelTask(new DateTimeProvider())
        {
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            EstimatedTime = 5,
            InvestedTime = 2,
            Epic = _epic,
            DueDate = new DateTime(2025, 10, 20),
        };
        _taskEpicless = new PanelTask(new DateTimeProvider())
        {
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            EstimatedTime = 5,
            InvestedTime = 2,
            DueDate = new DateTime(2025, 10, 20),
        };
        
        IDateTimeProvider dataTimeProviderFake = new DateTimeProviderFake(new DateTime(2019, 09, 17));
        
        _expiredTask = new PanelTask(dataTimeProviderFake)
        {
            Id = 1,
            Title = "Expired Task",
            Priority = "Urgent",
            Description = "Do homework",
            EstimatedTime = 5,
            InvestedTime = 2,
            DueDate = new DateTime(2020, 10, 20),
            commentList = new List<Comment>()
        };
        _repositoryPanel = new RepositoryDBPanel(_context);
        _repositoryPanelTask = new RepositoryDBPanelTask(_context);
        _panelTaskLogic = new PanelTaskLogic(new RepositoryComment(), _repositoryPanelTask);
        _panelLogic = new PanelLogic(_repositoryPanel, _repositoryPanelTask);
        
        
        _comment1 = new Comment()
        {
            Resolved = false,
            Description = "tasks that are expired"
            
        };
        _comment2 = new Comment()
        {
            Resolved = false,
            Description = "tasks that are expired"
            
        };
    }

    [TestMethod]
    public void CreatePanelOk()
    {
        _panelLogic.CreatePanel(_panel);
        Assert.IsTrue(_panelLogic.GetPanels().Contains(_panel));
    }

    [TestMethod]
    public void CreatePanelNoTeamError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() => { _panelLogic.CreatePanel(_panelWithoutTeam); },
            "Panel must be associated with a team");
        Assert.AreEqual("Panel must be associated with a team", exc.Message);
    }

    [TestMethod]
    public void RemovePanelOk()
    {
        _panel.Creator = _creator;
        _panelLogic.CreatePanel(_panel);
        _panelLogic.RemovePanel(_creator, _panel);
        Assert.IsFalse(_panelLogic.GetPanels().Contains(_panel));
    }

    [TestMethod]
    public void RemovePanelError()
    {
        _panelLogic.CreatePanel(_panel);
        Exception exc = Assert.ThrowsException<ArgumentException>(() => { _panelLogic.RemovePanel(_noCreator, _panel); },
            "Only the creator or an admin can remove a panel");
    }

    [TestMethod]
    public void ModifyPanelOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.ModifyPanel(_panel, _panel2);
        Assert.AreEqual("New Name", _panel.Name);
    }

    [TestMethod]
    public void CreateTaskOk()
    {
        _panelLogic.CreateTask(_panel, _task1);
        Assert.IsTrue(_panel.PanelTaskList.Contains(_task1));
    }
    

    [TestMethod]
    public void MoveToExpiredPanelOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreatePanel(_panel2);
        PanelTask notOutdatedTask = new PanelTask(new DateTimeProvider())
        {
            _dateTimeProvider = _dataTimeProvider,
            Title = "Not outdated task",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025, 10, 20),
            commentList = new List<Comment>()
        };
        _panelLogic.CreateTask(_panel2, _expiredTask);
        _panelLogic.CreateTask(_panel2, notOutdatedTask);
        _panelLogic.CheckExpiredTasks();
        Assert.IsTrue(_panel.PanelTaskList.Contains(_expiredTask));
        Assert.IsTrue(!_panel.PanelTaskList.Contains(notOutdatedTask));
        _panelLogic.CheckExpiredTasks();
        Assert.IsTrue(_panel.PanelTaskList.Count == 1);
    }

    [TestMethod]
    public void FindPanelByIdOk()
    {
        _panelLogic.CreatePanel(_panel);
        Panel findedPanel = _panelLogic.findPanelById(_panel.Id);
        Assert.AreSame(findedPanel, _panel);
    }

    [TestMethod]
    public void FindTaskByIdOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        PanelTask findedTask = _panelLogic.findTaskById(_task1.Id);
        Assert.AreSame(findedTask, _task1);
    }

    [TestMethod]
    public void ModifyTaskOk()
    {
        _panelLogic.CreateTask(_panel, _task1);
        _panelLogic.ModifyTask(_task1, _taskModifier);
        Assert.AreEqual(_task1.Title, _taskModifier.Title);
        Assert.AreEqual(_task1.Priority, _taskModifier.Priority);
        Assert.AreEqual(_task1.Description, _taskModifier.Description);
        Assert.AreEqual(_task1.DueDate, _taskModifier.DueDate);
    }

    [TestMethod]
    public void DeleteAllCommentRelatedToATask()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        _panelTaskLogic.AddComment(_task1, _comment1);
        _panelTaskLogic.AddComment(_task1, _comment2);
        _panelLogic.RemoveTaskPermanently(_user, _panel, _task1);
        Assert.IsFalse(_task1.commentList.Contains(_comment1));
        Assert.IsFalse(_task1.commentList.Contains(_comment2));
        
    }
    
    [TestMethod]
    public void DeleteAllTaskRelatedToAPanel()
    {
        
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        _panelTaskLogic.AddComment(_task1, _comment1);
        _panelTaskLogic.AddComment(_task1, _comment2);
        _panelLogic.RemovePanel(_user, _panel);
        Assert.IsFalse(_panelLogic.GetTasks().Contains(_task1));
        Assert.IsFalse(_task1.commentList.Contains(_comment1));
        Assert.IsFalse(_task1.commentList.Contains(_comment2));
        
    }
    

    [TestMethod]
    public void ReactivateTaskOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _expiredTask);
        _panelLogic.CheckExpiredTasks();
        _panelLogic.ReactivateTask(_user, _expiredTask);
        Assert.IsTrue(!_panel.PanelTaskList.Contains(_expiredTask)); ;
    }
    
    [TestMethod]
    public void ReactivateTaskError()
    {
        _user.Type = TypeUser.User;
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _panelLogic.ReactivateTask(_user, _task1);
        }, "Only an admin can reactivate a task");
        Assert.AreEqual("Only an admin can reactivate a task", exc.Message);
    }

    [TestMethod]
    public void CheckUserIsCreatorOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panel.Creator = _creator;
        Assert.IsTrue(_panelLogic.CheckUserIsCreator(_creator));
        Assert.IsFalse(_panelLogic.CheckUserIsCreator(_noCreator));
    }

    [TestMethod]
    public void GetPanelsOk()
    {
        List <Panel> panels= _panelLogic.GetPanels();
        Assert.AreEqual(0, panels.Count);
        
    }
    
    [TestMethod]
    public void GetTasksOk()
    {
        List <PanelTask> tasks= _panelLogic.GetTasks();
        Assert.AreEqual(0, tasks.Count);
        
    }
    
    [TestMethod]
    public void ExistsPanelOk()
    {
        _panelLogic.CreatePanel(_panel);
        Assert.IsTrue(_panelLogic.ExistsPanel(_panel));
    }
    
    [TestMethod]
    public void ExistsTaskOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        Assert.IsTrue(_panelLogic.ExistsTask(_task1));
    }

    [TestMethod]
    public void EstimatedTimeOk()
    {
        _panelLogic.CreatePanel(_panel3);
        _panelLogic.CreatePanel(_panel2);
        _panelLogic.CreateTask(_panel2, _task1);
        _panelLogic.CreateTask(_panel3, _task2);
        int expectedTotalTime = _task1.EstimatedTime + _task2.EstimatedTime;
        var result = _panelLogic.CalculateTotalEstimatedTime(_epic);
        Assert.AreEqual(expectedTotalTime, result);
    }

    [TestMethod]
    public void EstimatedTimeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var res = _panelLogic.CalculateTotalEstimatedTime(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }

    [TestMethod]
    public void EstimatedTimeZeroEpic()
    {
        var result = _panelLogic.CalculateTotalEstimatedTime(_epic); 
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void InvestedTimeOk()
    {
        _panelLogic.CreatePanel(_panel3);
        _panelLogic.CreatePanel(_panel2);
        _panelLogic.CreateTask(_panel2, _task1);
        _panelLogic.CreateTask(_panel3, _task2);
        int investedTotalTime = _task1.InvestedTime + _task2.InvestedTime;
        var result = _panelLogic.CalculateTotalInvestedTime(_epic);
        Assert.AreEqual(investedTotalTime, result);
    }
    
    [TestMethod]
    public void InvestedTimeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var res = _panelLogic.CalculateTotalInvestedTime(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }

    [TestMethod]
    public void InvestedTimeZeroEpic()
    {
        var result = _panelLogic.CalculateTotalInvestedTime(_epic);
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void RemainingTimeOk()
    {
        _panelLogic.CreatePanel(_panel3);
        _panelLogic.CreatePanel(_panel2);
        _panelLogic.CreateTask(_panel2, _task1);
        _panelLogic.CreateTask(_panel3, _task2);
        int expectedRemainingTime = (_task1.EstimatedTime + _task2.EstimatedTime) - (_task1.InvestedTime + _task2.InvestedTime);
        var result = _panelLogic.CalculateRemainingTime(_epic);
        Assert.AreEqual(expectedRemainingTime, result);
    }
    [TestMethod]
    public void RemainingTimePositiveOk()
    {
        _panelLogic.CreatePanel(_panel3);
        _panelLogic.CreatePanel(_panel2);
        _panelLogic.CreateTask(_panel2, _task1);
        _panelLogic.CreateTask(_panel3, _task2);
        int expectedRemainingTime = Math.Max(_task1.EstimatedTime - _task1.InvestedTime,0) + Math.Max(_task2.EstimatedTime - _task2.InvestedTime,0);
        var result = _panelLogic.CalculateRemainingTimePositive(_epic);
        Assert.AreEqual(expectedRemainingTime, result);
    }
    [TestMethod]
    public void CalculateTotalEstimatedTimeNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var result = _panelLogic.CalculateTotalEstimatedTime(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }
    
    [TestMethod]
    public void CalculateTotalInvestedTimeNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var result = _panelLogic.CalculateTotalInvestedTime(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }
    [TestMethod]
    public void CalculateTotalOverdueHoursOk()
    {
        _panelLogic.CreatePanel(_panel3);
        _panelLogic.CreateTask(_panel3, _task3);
        int expectedRemainingTime = Math.Max(_task3.InvestedTime - _task3.EstimatedTime, 0);
        var result = _panelLogic.CalculateTotalOverdueHours(_epic);
        Assert.AreEqual(expectedRemainingTime, result);
    }
    [TestMethod]
    public void CalculateTotalOverdueHoursErrorNull()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var result = _panelLogic.CalculateTotalOverdueHours(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }


    
    [TestMethod]
    public void RemainingTimeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var res = _panelLogic.CalculateRemainingTime(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }

    [TestMethod]
    public void GetAccessibleTasksOk()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        _panelLogic.CreateTask(_panel, _task2);
        List<PanelTask> accessibleTasks = _panelLogic.GetAccessibleTasks(_user);
        Assert.IsTrue(accessibleTasks.Count == 2);

    }
    
    [TestMethod]
    public void GetAccessibleTasksInvalidId()
    {
        _panel.Id = 1;
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        _panelLogic.CreateTask(_panel, _task2);
        List<PanelTask> accessibleTasks = _panelLogic.GetAccessibleTasks(_user);
        Assert.IsTrue(accessibleTasks.Count == 0);

    }
    
    [TestMethod]
    public void RemainingTimePositiveNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            var res = _panelLogic.CalculateRemainingTimePositive(null);
        }, "Epic cannot be null");
        Assert.AreEqual("Epic cannot be null", exc.Message);
    }

    [TestMethod]
    public void EpicHasTasksTrue()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task1);
        Assert.IsTrue( _panelLogic.EpicHasTasks(_epic));
    }
    [TestMethod]
    public void EpicHasTasksFalse()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _taskEpicless);
        Assert.IsFalse( _panelLogic.EpicHasTasks(_epic));
    }
    
    [TestMethod]
    public void EpicHasCompleteTasksTrue()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task3);
        Assert.IsTrue( _panelLogic.EpicHasCompleteTasks(_epic));
    }
    
    [TestMethod]
    public void EpicHasCompleteTasksFalse()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task2);
        Assert.IsFalse( _panelLogic.EpicHasCompleteTasks(_epic));
    }
    
    [TestMethod]
    public void EpicHasIncompleteTasksTrue()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task2);
        Assert.IsTrue( _panelLogic.EpicHasIncompleteTasks(_epic));
    }
    
    [TestMethod]
    public void EpicHasIncompleteTasksFalse()
    {
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel, _task3);
        Assert.IsFalse( _panelLogic.EpicHasIncompleteTasks(_epic));
    }
    
}