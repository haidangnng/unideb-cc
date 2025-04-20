using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth;

public class RegisterDto
{
    [Required] public string Username { get; set; } = String.Empty;
    [Required] public string Email { get; set; } = String.Empty;
    [Required] public string Password { get; set; } = String.Empty;
    [Required] public string UserRole { get; set; } = String.Empty;
}