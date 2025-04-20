
using QuestionLair.API.Data;
using QuestionLair.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Courses;
using Shared.Models.Courses;

namespace API.Services.CourseService;
public class CourseService : ICourseService
{
    private readonly AppDbContext _context;

    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course> CreateCourseAsync(CourseCreateDto dto, int teacherUserId)
    {
        var teacherProfile = await _context.TeacherProfiles
            .FirstOrDefaultAsync(p => p.Id == teacherUserId);

        if (teacherProfile == null) throw new Exception("Teacher profile not found");

        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            TeacherCourses = new List<TeacherCourse>
            {
                new TeacherCourse { TeacherProfileId = teacherProfile.Id }
            }
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task EnrollStudentAsync(int courseId, int studentUserId)
    {
        var studentProfile = await _context.StudentProfiles
            .FirstOrDefaultAsync(p => p.Id == studentUserId);

        if (studentProfile == null) throw new Exception("Student profile not found");

        var course = await _context.Courses.FindAsync(courseId);

        if (course == null) throw new Exception("Course not found");

        var exists = await _context.StudentCourses
            .AnyAsync(sc => sc.CourseId == courseId && sc.StudentProfileId == studentProfile.Id);

        if (exists) throw new Exception("Student profile already enrolled");

        _context.StudentCourses.Add(new StudentCourse
        {
            CourseId = courseId,
            StudentProfileId = studentProfile.Id
        });

        await _context.SaveChangesAsync();
    }

    public async Task<List<Course>> GetCoursesForStudent(int studentUserId)
    {
        var studentProfile = await _context.StudentProfiles
            .Include(p => p.Courses)
            .ThenInclude(sc => sc.Course)
            .FirstOrDefaultAsync(p => p.Id == studentUserId);

        return studentProfile?.Courses != null
            ? studentProfile.Courses.Select(c => c.Course).ToList()
            : new List<Course>();


    }

    public async Task<List<Course>> GetCoursesForTeacher(int teacherUserId)
    {
        var teacherProfile = await _context.TeacherProfiles
            .Include(p => p.Courses)
            .ThenInclude(tc => tc.Course)
            .FirstOrDefaultAsync(p => p.Id == teacherUserId);


        return teacherProfile?.Courses != null
            ? teacherProfile.Courses.Select(c => c.Course).ToList()
            : new List<Course>();
    }
}