using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Domain;

public class Panel
{
    public int Id { get; set; }
    private string? _name;
    public string? Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Name cannot be null or empty");
            }
            _name = value;
        }
    }

    public int CreatorId { get; set; }
    private User _creator;

    public User Creator
    {
        get=> _creator;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("Creator cannot be null");
            }
            _creator = value;
        }
    }
    public Team? Team { get; set; }
    public string? Description { get; set; }
    public List<PanelTask> PanelTaskList { get; set; } = new List<PanelTask>();


    public void addTask(PanelTask task)
    {
        if (task == null)
        {
            throw new ArgumentException("Task cannot be null");
        }
        PanelTaskList.Add(task);
    }

    public void removeTask(PanelTask task)
    {
        PanelTaskList.Remove(task);
    }
}