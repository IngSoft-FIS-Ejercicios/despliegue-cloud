using Domain;
using Logic;
using Repository;
using RepositoryTests.Context;
using static Logic.PanelTaskLogic;

namespace LogicTest;

[TestClass]
public class PanelTaskLogicTest
{
    private PanelTask _task;
    private PanelTaskLogic _logicTask;
    private Comment _comment;
    private RepositoryComment _repoComment;
    private RepositoryDBPanelTask _repoTask;
    private User _user;
    private Epic _epic;
    private SqlContext _context;

    [TestInitialize]
    public void SetUp()
    {
        _epic = new Epic(new DateTimeProvider())
        {
            Id = 1,
            Title = "Epic 1",
            Priority = "Urgent",
            Description = "Study",
            DueDate = new DateTime(2025, 10, 20),
        };

        _comment = new Comment()
        {
            Id = 1,
            Resolved = false,
            CreatorId = 1,
            Description = "Comment description",
            ResolverId = null,
            DateResolution = null
        };
        
        _task = new PanelTask(new DateTimeProvider())
        {
            Id = 1,
            Title = "Task 1",
            Priority = "Urgent",
            Description = "Do homework",
            DueDate = new DateTime(2025,10,20),
            EstimatedTime = 5,
            InvestedTime = 2,
            Epic = _epic,
            commentList = new List<Comment>()
        };

        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repoTask = new RepositoryDBPanelTask(_context);
        
        _repoComment = new RepositoryComment()
        {
            Comments = new List<Comment>()
        };
        
        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        

        _logicTask = new PanelTaskLogic(_repoComment, _repoTask);


    }
    
    [TestMethod]
    public void AddComentOk()
    {
        _logicTask.AddComment(_task, _comment);
        Assert.IsTrue(_repoComment.Comments.Contains(_comment));
        Assert.IsTrue(_task.commentList.Contains(_comment));
    }

    
    [TestMethod]

    public void ResolutionOfCommentOk()
    {
        _logicTask.ResolveComment(_comment, _user);
        Assert.IsTrue(_comment.Resolved);
        Assert.AreEqual(_comment.ResolverId, _user.Id);
        
    }

    [TestMethod]

    public void ResolutionOfCommentError()
    {
        _logicTask.ResolveComment(_comment, _user);
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _logicTask.ResolveComment(_comment, _user);
        }, "Comment has already been resolved");
        Assert.AreEqual("Comment has already been resolved", exc.Message);

    }
    
    [TestMethod]
    public void FindCommentByIdOk()
    {
        _repoComment.Add(_comment);
        Comment comment = _logicTask.FindCommentById(1);
        Assert.AreEqual(_comment, comment);
    }
    
    [TestMethod]
    public void FindCommentByIdError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _logicTask.FindCommentById(1);
        }, "Comment not found");
        Assert.AreEqual("Comment not found", exc.Message);
    }

    [TestMethod]
    public void InvestTimeOk()
    {
        int time = 3;
        _repoTask.Add(_task);
        _logicTask.InvestTime(_task, time);
        Assert.AreEqual(5, _task.InvestedTime);
    }

    [TestMethod]
    public void MarkAsDoneOk()
    {
        _repoTask.Add(_task);
        _logicTask.MarkAsDone(_task);
        Assert.AreEqual(_task.State, StateTask.Finished);
    }

}