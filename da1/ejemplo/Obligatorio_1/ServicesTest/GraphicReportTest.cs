using BlazorBootstrap;
using Domain;
using Logic;
using Moq;
using Logic;
using Repository;
using RepositoryTests.Context;

namespace LogicTest;

[TestClass]
public class GraphicReportTest
{
    private Panel _panel;
    private PanelLogic _panelLogic;
    private PanelTaskLogic _taskLogic;
    private PanelTask _task1;
    private PanelTask _task2;
    private PanelTask _task3;
    private PanelTask _task4;
    private PanelTask _task5;
    private PanelTask _task6;
    private Epic _epic;
    private EpicLogic _epicLogic;
    private RepositoryDBPanel _repositoryPanel;
    private RepositoryDBPanelTask _repositoryPanelTask;
    private RepositoryDBEpic _repositoryEpic;
    private GraphicReport _graphicReport;
    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        
        _repositoryPanel = new RepositoryDBPanel(_context);
        _repositoryPanelTask = new RepositoryDBPanelTask(_context);
        _repositoryEpic = new RepositoryDBEpic(_context);
        
        _panel = new Panel()
        {
            Name = "New Name",
            Creator = new User(),
            Team = new Team(),
            Description = "New Description",
            PanelTaskList = new List<PanelTask> { }
        };
        
        _epic = new Epic(new DateTimeProvider())
        {
            Title = "Epic 1",
            Priority = "Urgent",
            Description = "Study",
            DueDate = new DateTime(2025, 10, 20)
        };

        _task1 = new PanelTask()
        {
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            State = StateTask.Unfinished,
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 50,
            InvestedTime = 22,
            Epic = _epic,
        };

        _task2 = new PanelTask()
        {
            Title = "Task 2",
            Priority = "Urgent",
            Description = "Do homework",
            State = StateTask.Unfinished,
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 100,
            InvestedTime = 60,
            Epic = _epic,
        };
        
        _task3 = new PanelTask()
        {
            Title = "Task 3",
            Priority = "Urgent",
            Description = "Do homework",
            State = StateTask.Unfinished,
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 100,
            InvestedTime = 200,
            Epic = _epic,
        };
        
        _task4 = new PanelTask()
        {
            Title = "Task 4",
            Priority = "Urgent",
            Description = "Do homework",
            State = StateTask.Finished,
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 50,
            InvestedTime = 20,
            Epic = _epic,
        };

        _task5 = new PanelTask()
        {
            Title = "Task 5",
            Priority = "Urgent",
            Description = "Do homework",
            State = StateTask.Finished,
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 100,
            InvestedTime = 100,
            Epic = _epic,
        };
        
        _task6 = new PanelTask()
        {
            Title = "Task 6",
            Priority = "Urgent",
            Description = "Do homework",
            State = StateTask.Finished,
            DueDate = new DateTime(2025, 10, 20),
            EstimatedTime = 100,
            InvestedTime = 200,
            Epic = _epic,
        };

        _repositoryPanel = new RepositoryDBPanel(_context);
        _repositoryPanelTask = new RepositoryDBPanelTask(_context);
        _panelLogic = new PanelLogic(_repositoryPanel, _repositoryPanelTask);
        _epicLogic = new EpicLogic(_repositoryEpic);
        _graphicReport = new GraphicReport(_panelLogic);
        
        _epicLogic.CreateEpic(_epic);
        _panelLogic.CreatePanel(_panel);
        _panelLogic.CreateTask(_panel,_task1);
        _panelLogic.CreateTask(_panel,_task2);
        _panelLogic.CreateTask(_panel,_task3);
        _panelLogic.CreateTask(_panel,_task4);
        _panelLogic.CreateTask(_panel,_task5);
        _panelLogic.CreateTask(_panel,_task6);
    }

    [TestMethod]
    public void GetUnfinishTasksLabelsOk()
    {
        DataGraphicReportTasks data = _graphicReport.GetUnfinishTasks(_epic);
        Assert.AreEqual(3, data.labels.Count);
    }
    [TestMethod]
    public void GetUnfinishTasksDatasetsOk()
    {
        DataGraphicReportTasks data = _graphicReport.GetUnfinishTasks(_epic);
        Assert.AreEqual(3, data.datasets.Count);
    }

    [TestMethod]
    public void GetFinishTasksLabelsOk()
    {
        DataGraphicReportTasks data = _graphicReport.GetFinishTasks(_epic);
        Assert.AreEqual(3, data.labels.Count);
        
    }
    
    [TestMethod]
    public void GetFinishTasksDatasetsOk()
    {
        DataGraphicReportTasks data = _graphicReport.GetFinishTasks(_epic);
        Assert.AreEqual(5, data.datasets.Count);
        
    }
   
}
