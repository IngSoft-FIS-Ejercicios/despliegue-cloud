using Blazored.LocalStorage;
using Domain;
using Logic;

public class SessionLogic
{
    private readonly UserLogic _userLogic;
    private readonly ILocalStorageService _localStorage;
    private User? _currentUser;
    public bool IsInitialized { get; private set; } = false;

    public SessionLogic(UserLogic userLogic, ILocalStorageService localStorage)
    {
        _userLogic = userLogic;
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        var storedUser = await _localStorage.GetItemAsync<User>("loggedUser");
        _currentUser = storedUser;
        IsInitialized = true; 
    }

    public async Task Login(string username, string password)
    {
        _currentUser = _userLogic.LogIn(username, password);
        if (_currentUser != null)
        {
            await _localStorage.SetItemAsync("loggedUser", _currentUser);
        }
        IsInitialized = true;
    }

    public bool IsUserActive()
    {
        return _currentUser is not null;
    }

    public async Task LogOut()
    {
        _currentUser = null;
        IsInitialized = false;
        await _localStorage.RemoveItemAsync("loggedUser");
    }

    public User GetCurrentUser()
    {

        return _currentUser;
    }
}