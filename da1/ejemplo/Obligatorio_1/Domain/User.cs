using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Domain;


public enum ErrorCodePassword
{
    Ok,
    Empty,
    NoUppercase,
    NoLowercase,
    Short,
    NoDigit,
    NoSpecialCharacter
}

public enum TypeUser
{
    Admin,
    User
}

public static class PasswordErrorCodeExtensions
{
    public static string GetMessage(this ErrorCodePassword errorCode)
    {
        return errorCode switch
        {
            ErrorCodePassword.Empty => "Password cannot be empty",
            ErrorCodePassword.NoUppercase => "Password must contain at least one uppercase letter",
            ErrorCodePassword.NoLowercase => "Password must contain at least one lowercase letter",
            ErrorCodePassword.Short => "Password must contain at least 8 characters",
            ErrorCodePassword.NoDigit => "Password must contain at least one digit",
            ErrorCodePassword.NoSpecialCharacter => "Password must contain at least one special character",
            ErrorCodePassword.Ok => string.Empty
        };
    }
}


public class User
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
            
            if (value.Contains(" "))
            {
                throw new ArgumentException("Name cannot contain spaces");
            }

            if (!validate(value))
            {
                throw new ArgumentException("Name cannot contain numbers or special characters");
            }

            

            _name = value;
        }
    }

    private string? _surname;

    public string? Surname
    {
        get => _surname;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Surname cannot be null or empty");
            }

            if (value.Contains(" "))
            {
                throw new ArgumentException("Surname cannot contain spaces");
            }
            
            if (!validate(value))
            {
                throw new ArgumentException("Surname cannot contain numbers or special characters");
            }
            
            _surname = value;
        }
    }

    private string? _email;

    public string? Email
    {
        get => _email;
        set 
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Email cannot be null or empty");
            }

            if (!validateEmail(value))
            {
                throw new ArgumentException("Email is not valid");
            }

            
            _email = value;
        }

    }
    public TypeUser Type { get; set; } = TypeUser.User;
    private DateTime _birthDate;

    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (value >= DateTime.Now)
            {
                throw new ArgumentException("Birth date must be in the past");
            }
            
            DateTime formatedDate = DateTime.ParseExact(value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            _birthDate = formatedDate;
        }
        
    }


    private String? _password;
    public String? Password
    {
        get => _password;
        set
        {
            var errorCode = ValidatePassword(value);
            if (errorCode != ErrorCodePassword.Ok)
            {
                throw new ArgumentException(errorCode.GetMessage());
            }

            _password = value;
        }
    }
    
    public List<PanelTask> Trashpaper { get; } = new List<PanelTask>();
    public List<Team> TeamsList { get; } = new List<Team>();
    


    private bool validate(string text)
    {
        // Check if the string contains any number
        foreach (char c in text)
        {
            if (char.IsDigit(c))
            {
                return false;
            }
        }
        // Check if the string contains any special character
        foreach (char c in text)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }

        return true;
    }
    
    private static bool validateEmail(string email)
    { 
        string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

        return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
    }

    private static ErrorCodePassword ValidatePassword(string password)
    {
        // Password cannot be null or empty
        if (string.IsNullOrEmpty(password))
        {
            return ErrorCodePassword.Empty;
        }
            
        // Password does not contains uppercase letter
        if (!password.Any(char.IsUpper))
        {
            return ErrorCodePassword.NoUppercase;
        }
            
        //Password does not contain lowercase letter
        if (!password.Any(char.IsLower))
        {
            return ErrorCodePassword.NoLowercase;
        }
            
        //Password must be at least 8 characters long
        if (password.Length < 8)
        {
            return ErrorCodePassword.Short;
        }
            
        //Password must contain at least one digit
        if (!password.Any(char.IsNumber))
        {
            return ErrorCodePassword.NoDigit;
        }
            
        //Password must contain at least one special character
        if (password.All(char.IsLetterOrDigit))
        {
            return ErrorCodePassword.NoSpecialCharacter;
        }

        return ErrorCodePassword.Ok;
    }
    
    public bool IsValidName(string text)
    {
        return validate(text);
    }

    public override string ToString()
    {
        return this.Name + " " + this.Surname;
    }
    
}