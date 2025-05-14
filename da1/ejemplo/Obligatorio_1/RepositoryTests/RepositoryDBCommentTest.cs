using Domain;
using Repository;
using RepositoryTests.Context;

namespace RepositoryTests;

[TestClass]
public class RepositoryDBCommentTest
{
    private RepositoryDBComment _repositoryComment;
    private Comment _comment;
    private SqlContext _context;

    [TestInitialize]
    public void SetUp()
    {
        _context = SqlContextFactory.CreateMemoryContext();
        _context.Database.EnsureDeleted();
        _repositoryComment = new RepositoryDBComment(_context);
        
        _comment = new Comment()
        {
            Id = 1,
            Resolved = false,
            Description = "This is a comment",
            CreatorId = 1,
            DateResolution = null
        };
    }
    
    [TestMethod]
    public void AddValidComment()
    {
        _repositoryComment.Add(_comment);
        Assert.IsTrue(_repositoryComment.GetAll().Count == 1);
    }
    
    [TestMethod]
    public void AddCommentNullError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _repositoryComment.Add(null);
        }, "Comment cannot be null");
        
        Assert.AreEqual("Comment cannot be null", exc.Message);
    }
    
    
    [TestMethod]
    public void ModifyCommentOk()
    {
        Comment modifiedComment = new Comment()
        {
            Id = 2,
            Resolved = true,
            Description = "This is a modified comment",
            CreatorId = 1,
            ResolverId = 1,
            DateResolution = DateTime.Now
        };
        _repositoryComment.Add(_comment);
        _repositoryComment.Modify(_comment, modifiedComment);
        Assert.AreEqual(_repositoryComment.FindById(_comment.Id).Description, modifiedComment.Description);
    }
    
    [TestMethod]
    public void GetAllCommentsOk()
    {
        List<Comment> comments = _repositoryComment.GetAll();
        CollectionAssert.AreEqual(_repositoryComment.GetAll(), comments);
    }

    [TestMethod]
    public void FindCommentByIdOk()
    {
        _repositoryComment.Add(_comment);
        Comment comment = _repositoryComment.FindById(1);
        Assert.AreEqual(_comment.Description, comment.Description);
        Assert.AreEqual(_comment.Id, comment.Id);
        Assert.AreEqual(_comment.Resolved, comment.Resolved);
        Assert.AreEqual(_comment.CreatorId, comment.CreatorId);
        Assert.AreEqual(_comment.DateResolution, comment.DateResolution);
        
    }
}