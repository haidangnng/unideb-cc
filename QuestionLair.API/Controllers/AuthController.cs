using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using QuestionLair.API.Data;
using Shared.DTOs.Auth;
using Shared.Models.Users;
using Shared.Enums;

namespace QuestionLair.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                UserRole = Enum.Parse<UserRole>(dto.UserRole, true),
                PasswordHash = HashPassword(dto.Password),
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Save to generate user.Id

            if (user.UserRole == UserRole.Student)
            {
                StudentProfile studentProfile = new StudentProfile
                {
                    StudentId = user.Id.ToString(),
                    User = user
                };
                _context.StudentProfiles.Add(studentProfile);
            }
            else if (user.UserRole == UserRole.Teacher)
            {
                TeacherProfile teacherProfile = new TeacherProfile
                {
                    User = user
                };
                _context.TeacherProfiles.Add(teacherProfile);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var token = GenerateJwtToken(user);
            return Ok(new { token, refreshToken = user.RefreshToken });
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || user.PasswordHash != HashPassword(dto.Password))
            return Unauthorized("Invalid credentials");

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        Response.Cookies.Append("session", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true in production (must use HTTPS)
            SameSite = SameSiteMode.None, // Or `None` if cross-origin + credentials
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });

        return Ok(new TokenResponseDto { AccessToken = token, RefreshToken = user.RefreshToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiresAtUtc > DateTime.UtcNow);

        if (user == null) return Unauthorized("Invalid or expired refresh token");

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        return Ok(new { token, refreshToken = user.RefreshToken });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.AdminProfile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        object? profile = user.UserRole switch
        {
            UserRole.Student => new
            {
                user.StudentProfile?.StudentId,
                user.StudentProfile?.Major,
            },
            UserRole.Teacher => new
            {
                user.TeacherProfile?.Title,
                user.TeacherProfile?.Bio,
            },
            UserRole.Admin => new
            {
            },
            _ => null
        };

        var result = new
        {
            user.Id,
            user.Username,
            user.Email,
            user.UserRole,
            Profile = profile
        };

        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("session");
        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ??
                          User.FindFirst(JwtRegisteredClaimNames.Sub);

        Console.WriteLine($"[UserService] UserIdClaim: {userIdClaim}");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid token");

        var user = await _context.Users
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            .Include(u => u.AdminProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("User not found");

        object? profile = user.UserRole switch
        {
            UserRole.Student => new
            {
                user.StudentProfile?.StudentId,
                user.StudentProfile?.Major,
            },
            UserRole.Teacher => new
            {
                user.TeacherProfile?.Title,
                user.TeacherProfile?.Bio,
            },
            UserRole.Admin => new { },
            _ => null
        };

        var result = new
        {
            user.Id,
            user.Username,
            user.Email,
            user.UserRole,
            Profile = profile
        };

        return Ok(result);
    }
}