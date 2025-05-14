using Domain;

namespace DomainTests;

[TestClass]
public class PanelTaskTest
{
    private PanelTask _task;

    [TestInitialize]
    public void SetUp()
    {

        _task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025,10,20),
            EstimatedTime = 5,
            InvestedTime = 2,
            UserTrashpaperId = 1,
            commentList = new List<Comment>()
        };
    }
    
    [TestMethod]
    public void CreateTaskOk()
    {
        Assert.AreEqual(1, _task.Id);
        Assert.AreEqual("Task 1", _task.Title);
        Assert.AreEqual("Urgent", _task.Priority);
        Assert.AreEqual("Do homework", _task.Description);
        Assert.AreEqual(new DateTime(2025,10,20), _task.DueDate);
        Assert.AreEqual(5, _task.EstimatedTime);
        Assert.AreEqual(2, _task.InvestedTime);
        Assert.IsNotNull(_task.commentList);
        Assert.AreEqual(1, _task.UserTrashpaperId);
    }
    
    [TestMethod]
    public void ParameterlessConstructorOk()
    {
        PanelTask task = new PanelTask();
        Assert.IsNotNull(task);
    }
    
    [TestMethod]
    public void CreateTaskEmptyTitleError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.Title = "";
        }, "Title cannot be null or empty");
        
        Assert.AreEqual("Title cannot be null or empty", exc.Message);
    }
    
    
    [TestMethod]
    public void AddCommentOk()
    {
        Comment comment = new Comment();

        _task.addComment(comment);
        
        Assert.AreEqual(1, _task.commentList.Count);

    }

    [TestMethod]
    public void AddNullCommentError()
    {
        Comment comment = null;
        
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.addComment(comment);
        }, "Comment cannot be null");
        Assert.AreEqual("Comment cannot be null", exc.Message);
    }

    [TestMethod]

    public void CreateTaskInvalidPriorityError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.Priority = "Not valid";
        }, "Priority must be 'Urgent', 'Medium' or 'Low'");
        Assert.AreEqual("Priority must be 'Urgent', 'Medium' or 'Low'", exc.Message);
    }
    
    [TestMethod]
    public void CreateTaskInvalidDueDateError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.DueDate = new DateTime(2020,10,20);
        }, "Due date must be in the future");
        Assert.AreEqual("Due date must be in the future", exc.Message);
    }

    [TestMethod]
    public void AddEpicOk()
    {
        Epic epic = new Epic(new DateTimeProvider());
        _task.AddEpic(epic);
        Assert.AreEqual(epic, _task.Epic);
    }

    [TestMethod]
    public void RemoveEpicOk()
    {
        Epic epic = new Epic(new DateTimeProvider());
        _task.AddEpic(epic);
        _task.RemoveEpic();
        Assert.IsNull(_task.Epic);
    }

    [TestMethod]
    public void ModifyEpicOk()
    {
        Epic epic1 = new Epic(new DateTimeProvider())
        {
            Id = 1,
            Title = "Epic 1",
            Priority = "Urgent",
            Description = "Study",
            DueDate = new DateTime(2025,12,31)
        };
        
        Epic epic2 = new Epic(new DateTimeProvider())
        {
            Id = 2,
            Title = "Epic 2",
            Priority = PriorityEpic.Medium.ToString(),
            Description = "Work",
            DueDate = new DateTime(2025,12,31)
        };
        _task.AddEpic(epic1);
        _task.ModifyEpic(epic2);
        Assert.AreEqual(epic2, _task.Epic);
    }

    [TestMethod]
    public void SetNegativeEstimatedTimeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.EstimatedTime = -1;
        }, "Estimated time cannot be negative");

        Assert.AreEqual("Estimated time cannot be negative", exc.Message);
    }

    [TestMethod]
    public void SetNegativeInvestedTimeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.InvestedTime = -1;
        }, "Invested time cannot be negative");

        Assert.AreEqual("Invested time cannot be negative", exc.Message);
    }
    
    [TestMethod]
    public void InvestTimeOk()
    {
        _task.InvestTime(3);
        Assert.AreEqual(5, _task.InvestedTime);
    }

    [TestMethod]
    public void InvestTimeNegativeError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _task.InvestTime(-1);
        }, "Time must be greater than 0");
        Assert.AreEqual("Time must be greater than 0", exc.Message);
    }
    
    [TestMethod]
    public void MarkAsFinishedOk()
    {
        _task.FinishTask();
        Assert.AreEqual(StateTask.Finished, _task.State);
    }
    
    [TestMethod]
    public void CompareEstimatedInvested_Overestimated()
    {
        var result = _task.CompareEstimatedInvested();
        Assert.AreEqual(EstimateComparison.Overestimated, result);
    }

    [TestMethod]
    public void CompareEstimatedInvested_Underestimated()
    {
        _task.EstimatedTime = 2;
        _task.InvestedTime = 5;
        var result = _task.CompareEstimatedInvested();
        Assert.AreEqual(EstimateComparison.Underestimated, result);
    }

    [TestMethod]
    public void CompareEstimatedInvested_Ok()
    {
        _task.EstimatedTime = 4;
        _task.InvestedTime = 4;
        var result = _task.CompareEstimatedInvested();
        Assert.AreEqual(EstimateComparison.Ok, result);
    }
}