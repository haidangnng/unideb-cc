using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using QuestionLair.API.Data;
using Shared.DTOs.Auth;
using Shared.Models.Users;
using Shared.Enums;

namespace QuestionLair.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UserController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ??
                          User.FindFirst(JwtRegisteredClaimNames.Sub);

        Console.WriteLine($"[UserService] UserIdClaim: {userIdClaim}");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid token");

        var user = await _context.Users
            .Include(u => u.StudentProfile)
            .Include(u => u.TeacherProfile)
            // .Include(u => u.AdminProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("User not found");

        object? profile = user.UserRole switch
        {
            UserRole.Student => new
            {
                user.StudentProfile?.StudentId,
                user.StudentProfile?.Major,
            },
            UserRole.Teacher => new
            {
                user.TeacherProfile?.Title,
                user.TeacherProfile?.Bio,
            },
            UserRole.Admin => new { },
            _ => null
        };

        var result = new
        {
            user.Id,
            user.Username,
            user.Email,
            user.UserRole,
            Profile = profile
        };

        return Ok(result);
    }

    [Authorize]
    [HttpGet("students")]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _context.Users
            .Where(u => u.UserRole == UserRole.Student)
            .Include(u => u.StudentProfile)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.UserRole,
                StudentId = u.StudentProfile!.StudentId,
                Major = u.StudentProfile.Major
            })
            .ToListAsync();

        return Ok(students);
    }

    [Authorize]
    [HttpGet("students/by-course/{courseId}")]
    public async Task<IActionResult> GetStudentByCourseId(int courseId)
    {
        var students = await _context.Users
               .Where(u => u.UserRole == UserRole.Student)
               .Include(u => u.StudentProfile)
                   .ThenInclude(sp => sp.Courses)
               .Where(u => u.StudentProfile.Courses.Any(sc => sc.CourseId == courseId))
               .Select(u => new
               {
                   u.Id,
                   u.Username,
                   u.Email,
                   u.UserRole,
                   StudentId = u.StudentProfile!.StudentId,
                   Major = u.StudentProfile.Major
               })
               .ToListAsync();

        return Ok(students);
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpGet("students/not-in-course/{courseId}")]
    public async Task<IActionResult> GetStudentsNotInCourse(int courseId)
    {
        var enrolledStudentIds = await _context.StudentCourses
            .Where(sc => sc.CourseId == courseId)
            .Select(sc => sc.StudentProfileId)
            .ToListAsync();

        var students = await _context.Users
            .Where(u => u.UserRole == UserRole.Student && !enrolledStudentIds.Contains(u.StudentProfile.Id))
            .Include(u => u.StudentProfile)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.UserRole,
                StudentId = u.StudentProfile.StudentId,
                Major = u.StudentProfile.Major
            })
            .ToListAsync();

        return Ok(students);
    }
}