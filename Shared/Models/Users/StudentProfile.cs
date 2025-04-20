using Shared.Models.Courses;

namespace Shared.Models.Users;
public class StudentProfile
{
    public int Id { get; set; }
    public string StudentId { get; set; } = String.Empty;
    public string Major { get; set; } = String.Empty;
    public required User User { get; set; }
    public List<StudentCourse> Courses { get; set; } = new();
}