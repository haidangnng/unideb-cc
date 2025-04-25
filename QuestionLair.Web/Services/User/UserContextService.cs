namespace QuestionLair.Web.Services.Users;
using QuestionLair.Web.Interfaces.Users;
using Shared.Models.Users;

public class UserContextService
{
    private readonly IUserService _userService;
    public User? CurrentUser { get; private set; }
    public bool IsInitialized => CurrentUser != null;

    public UserContextService(IUserService userService)
    {
        _userService = userService;
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
        }
    }

    public void Clear()
    {
        CurrentUser = null;
    }
}
