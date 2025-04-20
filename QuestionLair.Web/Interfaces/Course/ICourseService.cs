using Shared.DTOs.Courses;
using Shared.Models.Courses;

namespace QuestionLair.Web.Interfaces.Courses;

public interface ICourseClientService
{
    Task<TeacherCourseDTO?> CreateCourseAsync(CourseCreateDto dto);
    Task EnrollStudentAsync(int courseId);
    Task<List<Course>> GetMyCoursesAsync();
}