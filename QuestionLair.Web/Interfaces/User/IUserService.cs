using Shared.Models.Users;

namespace QuestionLair.Web.Interfaces.Users;

public interface IUserService
{
    Task<User?> GetCurrentUserAsync();
    Task<List<User>> GetAllStudents();
}