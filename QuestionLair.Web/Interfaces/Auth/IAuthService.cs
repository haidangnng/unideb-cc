using Shared.DTOs.Auth;

namespace QuestionLair.Web.Interfaces.Auth;
public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequestDTO login);
    Task LogoutAsync();
    Task<bool> TryRefreshTokenAsync();
}
