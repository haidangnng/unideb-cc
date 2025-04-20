using System.Text.Json.Serialization;
using Shared.Models.Users;

namespace Shared.Models.Courses;

public class TeacherCourse
{
    public int TeacherProfileId { get; set; }
    public TeacherProfile TeacherProfile { get; set; } = default!;

    public int CourseId { get; set; }

    [JsonIgnore]
    public Course Course { get; set; } = default!;
}
