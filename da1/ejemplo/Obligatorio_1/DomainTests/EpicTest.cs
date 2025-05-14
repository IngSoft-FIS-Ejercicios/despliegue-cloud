using Domain;

namespace DomainTests;

[TestClass]

public class EpicTest
{
    private Epic _epic;

    [TestInitialize]
    public void SetUp()
    {

        _epic = new Epic(new DateTimeProvider())
        {
            Id = 1,
            Title = "Epic 1",
            Priority = "Urgent",
            Description = "Study",
            DueDate = new DateTime(2025,12,31)
        };
    }

    [TestMethod]
    public void CreateEpicOk()
    {
        Assert.AreEqual(1, _epic.Id);
        Assert.AreEqual("Epic 1", _epic.Title);
        Assert.AreEqual("Urgent", _epic.Priority);
        Assert.AreEqual("Study", _epic.Description);
        Assert.AreEqual(new DateTime(2025, 12, 31), _epic.DueDate);
    }

    [TestMethod]
    public void ParameterlessConstructor()
    {
        Epic epic = new Epic();
        Assert.IsNotNull(epic);
    }

    [TestMethod]
    public void CreateEpicEmptyTitleError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _epic.Title = "";
        }, "Title cannot be null or empty");

        Assert.AreEqual("Title cannot be null or empty", exc.Message);
    }
    

    [TestMethod]

    public void CreateEpicInvalidPriorityError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _epic.Priority = "Not valid";
        }, "Priority must be 'Urgent', 'Medium' or 'Low'");
        Assert.AreEqual("Priority must be 'Urgent', 'Medium' or 'Low'", exc.Message);
    }

    [TestMethod]
    public void CreateEpicInvalidDueDateError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _epic.DueDate = new DateTime(2023,12,31);
        }, "Due date must be in the future");
        Assert.AreEqual("Due date must be in the future", exc.Message);
    }
}