using System.Globalization;
using System.Text.RegularExpressions;

namespace Domain;

public enum PriorityEpic
{
    Low,
    Medium,
    Urgent
}
public class Epic
{
    public IDateTimeProvider _dateTimeProvider;
    public int Id { get; set; }

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Title cannot be null or empty");
            }
            _title = value;
        }
    }

    private PriorityEpic _priority;
    public string Priority
    {
        get => _priority.ToString();
        set
        {
            if (!Enum.TryParse(value, true, out PriorityEpic priority))
            {
                throw new ArgumentException("Priority must be 'Urgent', 'Medium' or 'Low'");
            }
            _priority = priority;
        }
    }

    private string _description;
    public string Description
    {
        get => _description;
        set => _description = value;
    }

    private DateTime _dueDate;
    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            if (value < _dateTimeProvider.GetCurrentDateTime())
            {
                throw new ArgumentException("Due date must be in the future");
            }
            
            DateTime formated_dueDate = DateTime.ParseExact(value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            _dueDate = formated_dueDate;
        }
    }
    
    public Epic() : this(new DateTimeProvider())
    {
        
    }
    public Epic(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

}
