using Shared.Models.Users;

namespace Shared.Models.Courses;
public class StudentCourse
{
    public int StudentProfileId { get; set; }
    public StudentProfile StudentProfile { get; set; } = default!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
}
