using Shared.Models.Users;

namespace QuestionLair.Web.Interfaces.Users;

public interface IUserService
{
    Task<User?> GetCurrentUserAsync();
    Task<List<User>> GetAllStudents();
    Task<List<User>> GetStudentByCourseId(int courseId);
    Task<List<User>> GetStudentNotCourseId(int courseId);
}