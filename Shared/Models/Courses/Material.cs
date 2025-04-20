namespace Shared.Models.Courses;
public class Material
{
    public int Id { get; set; }
    public string Url { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public int UploadedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}