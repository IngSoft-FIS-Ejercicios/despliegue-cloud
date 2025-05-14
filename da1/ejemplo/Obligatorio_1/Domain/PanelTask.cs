using System.Globalization;
using System.Text.RegularExpressions;

namespace Domain;

public enum PriorityTask
{
    Low,
    Medium,
    Urgent
}

public enum EstimateComparison
{
    Overestimated,
    Underestimated,
    Ok
}

public enum StateTask
{
    Unfinished,
    Finished
}

public class PanelTask
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
    public StateTask State { get; set; } = StateTask.Unfinished;

    private int _estimatedTime;
    public int EstimatedTime
    {
        get => _estimatedTime;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Estimated time cannot be negative");
            }
            _estimatedTime = value;
        }
    }

    private int _investedTime;
    public int InvestedTime
    {
        get => _investedTime;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Invested time cannot be negative");
            }
            _investedTime = value;
        }
    }


    private PriorityTask _priority;

    public string Priority
    {
        get => _priority.ToString();
        set
        {
            if (!Enum.TryParse(value, true, out PriorityTask priority))
            {
                throw new ArgumentException("Priority must be 'Urgent', 'Medium' or 'Low'");
            }
            _priority = priority;
        }
    }
    public string Description { get; set; }
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
    public List<Comment> commentList { get; set; } = new List<Comment>();
    
    public int? UserTrashpaperId { get; set; }
    public Epic? Epic { get; set; }


    public PanelTask() : this(new DateTimeProvider())
    {
        
    }
    public PanelTask(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    public void addComment(Comment comment)
    {
        if (comment == null)
        {
            throw new ArgumentException("Comment cannot be null");
        }
        commentList.Add(comment);
    }
    
    public void AddEpic(Epic epic)
    {
        Epic = epic;
    }

    public void RemoveEpic()
    {
        Epic = null;
    }

    public void ModifyEpic(Epic epic)
    {
        Epic = epic;
    }
    
    public void InvestTime(int time)
    {
        if(time <= 0) throw new ArgumentException("Time must be greater than 0");
        InvestedTime += time;
    }

    public void FinishTask()
    {
        State = StateTask.Finished;
    }
    
    public EstimateComparison CompareEstimatedInvested()
    {

        if (EstimatedTime > InvestedTime)
        {
            return EstimateComparison.Overestimated;
        }
        else if (EstimatedTime < InvestedTime)
        {
            return EstimateComparison.Underestimated;
        }
        else
        {
            return EstimateComparison.Ok;
        }
        
    }

}
