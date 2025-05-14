using Domain;

namespace DomainTests;

[TestClass]
public class PasswordErrorCodeTests
{
    [TestMethod]
    public void PasswordErrorCodeOk()
    {
        string message = ErrorCodePassword.Ok.GetMessage();
        Assert.AreEqual("", message);
    }
    
    [TestMethod]
    public void PasswordErrorCodeEmpty()
    {
        string message = ErrorCodePassword.Empty.GetMessage();
        Assert.AreEqual("Password cannot be empty", message);
    }
    
    [TestMethod]
    public void PasswordErrorCodeNoUppercase()
    {
        string message = ErrorCodePassword.NoUppercase.GetMessage();
        Assert.AreEqual("Password must contain at least one uppercase letter", message);
    }
    
    [TestMethod]
    public void PasswordErrorCodeNoLowercase()
    {
        string message = ErrorCodePassword.NoLowercase.GetMessage();
        Assert.AreEqual("Password must contain at least one lowercase letter", message);
    }
    
    [TestMethod]
    public void PasswordErrorCodeShort()
    {
        string message = ErrorCodePassword.Short.GetMessage();
        Assert.AreEqual("Password must contain at least 8 characters", message);
    }
    
    [TestMethod]
    public void PasswordErrorCodeNoDigit()
    {
        string message = ErrorCodePassword.NoDigit.GetMessage();
        Assert.AreEqual("Password must contain at least one digit", message);
    }
    
    [TestMethod]
    public void PasswordErrorCodeNoSpecialCharacter()
    {
        string message = ErrorCodePassword.NoSpecialCharacter.GetMessage();
        Assert.AreEqual("Password must contain at least one special character", message);
    }
    
}