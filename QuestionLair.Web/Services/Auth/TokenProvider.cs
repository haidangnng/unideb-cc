using Blazored.LocalStorage;

namespace QuestionLair.Web.Services.Auth;

public class TokenProvider
{
    private readonly ILocalStorageService _localStorage;
    private string? _cachedToken;

    public TokenProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(_cachedToken))
            return _cachedToken;

        try
        {
            _cachedToken = await _localStorage.GetItemAsync<string>("accessToken");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[TokenProvider] Error accessing token: " + ex.Message);
        }

        return _cachedToken;
    }

    public void ClearToken()
    {
        _cachedToken = null;
    }
}