using Shared.DTOs.Courses;
using Shared.Models.Courses;

namespace QuestionLair.API.Interfaces;

public interface ICourseService
{
    Task<Course> CreateCourseAsync(CourseCreateDto dto, int teacherUserId);
    Task EnrollStudentAsync(int courseId, int studentId);
    Task<List<Course>> GetCoursesForStudent(int studentUserId);
    Task<List<Course>> GetCoursesForTeacher(int teacherUserId);
}