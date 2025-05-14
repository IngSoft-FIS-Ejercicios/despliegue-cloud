using System.Globalization;

namespace Domain;
using System.ComponentModel.DataAnnotations;

public class Team
{
    public int Id { get; set; }
    private string? _teamName;
    public string? TeamName
    {
        get => _teamName;
        set
        {
            if (string.IsNullOrEmpty(value.Trim()))
            {
                throw new ArgumentException("TeamName cannot be empty");
            }
            _teamName = value.Trim();
        }
    }

    private DateTime _teamCreationDate;

    public String? TeamCreationDate
    {
        get => _teamCreationDate.ToString("dd/MM/yyyy");
        set
        {
            if (!DateTime.TryParseExact(value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _teamCreationDate))
            {
                throw new ArgumentException("Date is invalid");
            }
        }
    }
    
    private string? _taskDescription;

    public string? TaskDescription
    {
        get => _taskDescription;
        set => _taskDescription = value?.Trim();
    }

    private int? _maxUsersAllowed;

    public int? MaxUsersAllowed
    {
        get => _maxUsersAllowed;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("MaxUsersAllowed cannot be null");
            }
            if (value <= 0)
            {
                throw new ArgumentException("MaxUsersAllowed must be greater than 0");
            }

            if (value < TeamUsersList.Count)
            {
                throw new ArgumentException(
                    "MaxUsersAllowed cant be lower than TeamUserList.Count, _teamUsersList.Count = " +
                    TeamUsersList.Count);
            }
            _maxUsersAllowed = value;
        }
    }

    public int AdministratorId { get; set; }
    private User? _administrator;

    public User? Administrator
    {
        get => _administrator;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("Administrator cannot be null");
            }
            _administrator = value;
        }
    }

    public List<User> TeamUsersList { get; set; } = new List<User>();

    public void addUser(User user)
    {
        if (_maxUsersAllowed <= TeamUsersList.Count)
        {
            throw new ArgumentException("You cant have more users than allowed, MaxUsersAllowed = "+MaxUsersAllowed);
        }
        TeamUsersList.Add(user);
    }

    public int numberOfUsers()
    {
        return TeamUsersList.Count;
    }
}