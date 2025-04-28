using Shared.Models.Courses;

namespace Shared.Models.Tests;
public class Test
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public int CreatedBy { get; set; } // Teacher UserId

    public List<TestQuestion> Questions { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<TestMaterial> TestMaterials { get; set; } = new();
}