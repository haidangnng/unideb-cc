using QuestionLair.API.Data;
using QuestionLair.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Courses;
using System.Security.Claims;

namespace QuestionLair.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICourseService _courseService;

    public CoursesController(AppDbContext context, ICourseService courseService)
    {
        _context = context;
        _courseService = courseService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ========================
    // Professors: Create Course
    // ========================
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var course = await _courseService.CreateCourseAsync(dto, GetUserId());
            var courseDto = new TeacherCourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
            };
            await transaction.CommitAsync();

            return Ok(courseDto);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();

            return BadRequest(e.Message);
        }
    }

    // ========================
    // Students: Enroll
    // ========================
    [HttpPost("{courseId}/enroll")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> EnrollStudent(int courseId)
    {
        await _courseService.EnrollStudentAsync(courseId, GetUserId());
        return Ok(new { message = "Enrolled successfully" });
    }

    [HttpPost("{courseId}/enroll/{studentId}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> EnrollStudentByTeacher(int courseId, int studentId)
    {
        await _courseService.EnrollStudentAsync(courseId, studentId);
        return Ok(new { message = $"Student {studentId} enrolled successfully" });
    }


    // ========================
    // Get Courses
    // ========================
    [HttpGet("my")]
    public async Task<IActionResult> GetMyCourses()
    {
        var userId = GetUserId();

        if (User.IsInRole("Student"))
        {
            var courses = await _courseService.GetCoursesForStudent(userId);
            return Ok(courses);
        }

        if (User.IsInRole("Teacher"))
        {
            var courses = await _courseService.GetCoursesForTeacher(userId);
            return Ok(courses);
        }

        return Forbid("Only students and teachers can access this endpoint.");
    }
}
