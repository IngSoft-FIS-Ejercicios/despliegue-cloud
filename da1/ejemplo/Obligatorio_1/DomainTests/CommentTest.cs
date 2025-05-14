using Domain;

namespace DomainTests;

[TestClass]
public class CommentTest
{
    private Comment _comment;
    private User _creator;

    [TestInitialize]
    public void SetUp()
    {
        _creator = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            Type = TypeUser.User,
            BirthDate = new DateTime(2024, 10, 20),
            Password = "Lucas20#"
        };
        
        _comment = new Comment()
        {
            Id = 1,
            Resolved = false,
            Description = "Comment description",
            CreatorId = _creator.Id,
            ResolverId = null,
            DateResolution = null
        };
    }
    
    [TestMethod]
    public void CreateCommentOk()
    {
        Assert.IsNotNull(_comment);
        Assert.AreEqual(1, _comment.Id);
        Assert.IsFalse(_comment.Resolved);
        Assert.AreEqual("Comment description", _comment.Description);
        Assert.IsNull(_comment.ResolverId);
        Assert.IsNull(_comment.DateResolution);
        Assert.AreEqual(_creator.Id, _comment.CreatorId);
    }

    [TestMethod]
    public void CreateCommentEmptyDescriptionError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _comment.Description = "";
        }, "Description cannot be null or empty");
        Assert.AreEqual("Description cannot be null or empty", exc.Message);
        
    }

    
}