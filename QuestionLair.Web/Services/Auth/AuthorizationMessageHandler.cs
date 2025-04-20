using System.Net.Http.Headers;

namespace QuestionLair.Web.Services.Auth;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly TokenProvider _tokenProvider;

    public AuthorizationMessageHandler(TokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"[AuthHandler] Token attached to: {request.RequestUri}");
        }
        else
        {
            Console.WriteLine("[AuthHandler] No token found; skipping Authorization header.");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
