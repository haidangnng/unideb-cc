namespace Shared.Models.Courses;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Many-to-many with students and teachers
    public List<StudentCourse> StudentCourses { get; set; } = new();
    public List<TeacherCourse> TeacherCourses { get; set; } = new();
}
