namespace QuestionLair.Web.Services.Users;

using Microsoft.AspNetCore.Components;
using QuestionLair.Web.Interfaces.Users;
using Shared.Models.Users;

public class UserContextService
{
    private readonly IUserService _userService;
    private NavigationManager _navigation;
    public User? CurrentUser { get; private set; }
    public bool IsInitialized => CurrentUser != null;

    public UserContextService(IUserService userService, NavigationManager navigation)
    {
        _userService = userService;
        _navigation = navigation;
    }

    public async Task InitializeAsync()
    {
        if (CurrentUser == null)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user != null)
            {
                CurrentUser = new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    UserRole = user.UserRole,
                };
            }
            else
            {
                _navigation.NavigateTo("/login");
            }
        }
    }

    public void Clear()
    {
        CurrentUser = null;
    }
}
