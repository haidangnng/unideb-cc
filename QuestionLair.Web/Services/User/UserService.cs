using Shared.Models.Users;
using QuestionLair.Web.Interfaces.Users;

namespace QuestionLair.Web.Services.Users;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("ApiClient");
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<User>("auth/me");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UserService] Failed to fetch user: {ex.Message}");
            return null;
        }
    }
}