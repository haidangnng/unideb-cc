using Shared.Enums;
namespace Shared.Models.Users;
public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
    public UserRole UserRole { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public TeacherProfile? TeacherProfile { get; set; }
    public AdminProfile? AdminProfile { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAtUtc { get; set; }
}