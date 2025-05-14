using Domain;

namespace DomainTests;

[TestClass]
public class UserTest
{
    private User _user;

    [TestInitialize]
    public void SetUp()
    {
        _user = new User()
        {
            Id = 1,
            Name = "Lucas",
            Surname = "Gonzalez",
            Email = "lucas2004@gmail.com",
            Type = TypeUser.User,
            BirthDate = new DateTime(2004, 10, 20),
            Password = "Lucas20#"
        };
    }
    
    [TestMethod]
    public void CreateUserOk()
    {
        Assert.IsNotNull(_user);
        Assert.AreEqual(1, _user.Id);
        Assert.AreEqual("Lucas", _user.Name);
        Assert.AreEqual("Gonzalez", _user.Surname);
        Assert.AreEqual("lucas2004@gmail.com", _user.Email);
        Assert.AreEqual(new DateTime(2004,10,20), _user.BirthDate);
        Assert.AreEqual("Lucas20#", _user.Password);
        Assert.AreEqual(_user.TeamsList.Count, 0);
        Assert.AreEqual(_user.Trashpaper.Count, 0);
    }

    [TestMethod]
    public void ValidateToStringOk()
    {
        Assert.AreEqual("Lucas Gonzalez", _user.ToString());
    }
    
    [TestMethod]
    public void ValidateUserIsAdminOk()
    {
        _user.Type = TypeUser.Admin;
        Assert.AreEqual("Admin", _user.Type.ToString());
    }

    [TestMethod]
    public void CreateUserEmptyNameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Name = "";
        }, "Name cannot be null or empty");
        
        Assert.AreEqual("Name cannot be null or empty", exc.Message);
        
    }
    
    [TestMethod]
    
    public void CreateUserInvalidCharactersNameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Name = "Lucas20#";
        }, "Name cannot contain numbers or special characters");
        
        Assert.AreEqual("Name cannot contain numbers or special characters", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserWithInvalidNameContainingUnderscoreError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Name = "Manuel_6";
        }, "Name cannot contain numbers or special characters");
        
        Assert.AreEqual("Name cannot contain numbers or special characters", exc.Message);
    }

    [TestMethod]

    public void CreateUserSpaceInNameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Name = "Al fredo";
        }, "Name cannot contain spaces");
        Assert.AreEqual("Name cannot contain spaces", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserEmptySurnameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Surname = "";
        }, "Surname cannot be null or empty");
        Assert.AreEqual("Surname cannot be null or empty", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserInvalidCharactersSurnameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Surname = "Gonzalez20#";
        }, "Surname cannot contain numbers or special characters");
        Assert.AreEqual("Surname cannot contain numbers or special characters", exc.Message);
    }

    [TestMethod]
    public void CreateUserSpaceInSurnameError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Surname = "Gon zalez";
        }, "Surname cannot contain spaces");
        Assert.AreEqual("Surname cannot contain spaces", exc.Message);
    }

    [TestMethod]

    public void CreateUserEmptyEmailError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Email = "";
        }, "Email cannot be null or empty");
        Assert.AreEqual("Email cannot be null or empty", exc.Message);
    }

    [TestMethod]

    // This test check the missing "@" character in the email
    public void CreateUserInvalidEmailError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Email = "lucas2004gmail.com";
        }, "Email is not valid");
        Assert.AreEqual("Email is not valid", exc.Message);
    }

    [TestMethod]
    // This test check the missing .com in the email
    public void CreateUserWithMissingDomainInEmailError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Email = "lucas2004@gmail";
        }, "Email is not valid");
        Assert.AreEqual("Email is not valid", exc.Message);
    }

    [TestMethod]
    // This test check the missing "direction" in the email
    public void CreateUserWithMissingLocalPartInEmailError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Email = "@gmail.com";
        }, "Email is not valid");
        Assert.AreEqual("Email is not valid", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserInvalidDateBirthDateError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.BirthDate = new DateTime(2025, 10, 20);
        }, "Birth date must be in the past");
        Assert.AreEqual("Birth date must be in the past", exc.Message);
    }

    [TestMethod]
    public void CreateUserEmptyPasswordError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Password = "";
        }, "Password cannot be empty");
        Assert.AreEqual("Password cannot be empty", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserNoUppercasePasswordError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Password = "lucas20#";
        }, "Password must contain at least one uppercase letter");
        Assert.AreEqual("Password must contain at least one uppercase letter", exc.Message);
    }
    
    [TestMethod]
    
    public void CreateUserLessThanEightCharactersPasswordError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Password = "Lucas20";
        }, "Password must contain at least 8 characters");
        Assert.AreEqual("Password must contain at least 8 characters", exc.Message);
    }

    [TestMethod]
    public void CreateUserNoLowercasePasswordError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Password = "LUCAS20#";
        }, "Password must contain at least one lowercase letter");
        Assert.AreEqual("Password must contain at least one lowercase letter", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserNoDigitPasswordError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Password = "LucasPepe#";
        }, "Password must contain at least one digit");
        Assert.AreEqual("Password must contain at least one digit", exc.Message);
    }
    
    [TestMethod]
    public void CreateUserNoSpecialCharacterPasswordError()
    {
        Exception exc = Assert.ThrowsException<ArgumentException>(() =>
        {
            _user.Password = "Lucas2004";
        }, "Password must contain at least one special character");
        Assert.AreEqual("Password must contain at least one special character", exc.Message);
    }
    
    [TestMethod]
    public void ValidateFunctionNameOk()
    {
        bool result = _user.IsValidName("Lucas");
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void ValidateFunctionNameWithDigitError()
    {
        bool result = _user.IsValidName("Lucas20");
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void ValidateFunctionNameWithSpecialCharacterError()
    {
        bool result = _user.IsValidName("Lucas#");
        Assert.IsFalse(result);
    }
    

    
}