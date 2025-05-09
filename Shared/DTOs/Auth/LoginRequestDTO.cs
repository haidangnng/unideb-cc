using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth;

public class LoginRequestDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}