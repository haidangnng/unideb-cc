using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuestionLair.Web.Components;
using MudBlazor.Services;
using QuestionLair.Web.Interfaces.Auth;
using QuestionLair.Web.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using QuestionLair.Web.Interfaces.Users;
using QuestionLair.Web.Services.Users;
using System.Net;
using QuestionLair.Web.Interfaces.Courses;
using QuestionLair.Web.Services.Courses;
using QuestionLair.Web.Interfaces.Materials;
using QuestionLair.Web.Services.Materials;
using QuestionLair.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<CookieContainer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("JWT")!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"]!,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("session", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationCore();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();


// builder.Services.AddHttpClient("ApiClient", client =>
// {
//     client.BaseAddress = new Uri("http://localhost:8080/");
// })
// .ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler
// {
//     UseCookies = true,
//     CookieContainer = sp.GetRequiredService<CookieContainer>(),
//     UseDefaultCredentials = false // important: do NOT send OS credentials
// });

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/");
})
.ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = sp.GetRequiredService<CookieContainer>(),
    UseDefaultCredentials = false
});

builder.Services.AddHttpClient("ApiLLM", client =>
{
    client.BaseAddress = new Uri("http://localhost:5500/");
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseClientService, CourseClientService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ITestService, TestService>();

// CONTEXT
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<ThemeContextService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

