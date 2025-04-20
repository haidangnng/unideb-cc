using Microsoft.AspNetCore.Components;
using QuestionLair.Web.Interfaces.Auth;
using Shared.DTOs.Auth;

namespace QuestionLair.Web.Services.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigation;

    public AuthService(IHttpClientFactory clientFactory, NavigationManager navigation)
    {
        _httpClient = clientFactory.CreateClient("ApiClient");
        _navigation = navigation;
    }

    public async Task<bool> LoginAsync(LoginRequestDTO login)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", login);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("auth/logout", null);
        _navigation.NavigateTo("/login", true);
    }

    public Task<bool> TryRefreshTokenAsync() => Task.FromResult(false);
}

