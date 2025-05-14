using Domain;
using Repository;

namespace RepositoryTests;

[TestClass]
public class RepositoryCommentTest
{

    private RepositoryComment _repositoryComment;
    private Comment _comment;

    [TestInitialize]

    public void SetUp()
    {
        _repositoryComment = new RepositoryComment()
        {
            Comments = new List<Comment>()
        };
        
        _comment = new Comment()
        {
            Id = 1,
            Resolved = false,
            Description = "This is a comment",
            ResolverId = null,
            DateResolution = null
        };
    }
    
    [TestMethod]
    public void AddValidComment()
    {
        _repositoryComment.Add(_comment);
        Assert.IsTrue(_repositoryComment.Comments.Contains(_comment));
        Assert.AreEqual(2, _repositoryComment.GetId());
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
            ResolverId = null,
            DateResolution = null
        };
        
        _repositoryComment.Modify(_comment, modifiedComment);
        Assert.AreEqual(_comment.Description, modifiedComment.Description);
    }
    
    [TestMethod]
    public void GetAllCommentsOk()
    {
        List<Comment> comments = _repositoryComment.GetAll();
        Assert.AreEqual(_repositoryComment.Comments, comments);
    }

    [TestMethod]
    public void FindCommentByIdOk()
    {
        _repositoryComment.Add(_comment);
        Comment comment = _repositoryComment.FindById(1);
        Assert.AreEqual(_comment, comment);
    }
}