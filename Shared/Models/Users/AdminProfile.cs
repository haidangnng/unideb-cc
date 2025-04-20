using Shared.Models.Users;

namespace Shared.Models;

public class AdminProfile
{
    public int UserId { get; set; }
    public required User User { get; set; }
}