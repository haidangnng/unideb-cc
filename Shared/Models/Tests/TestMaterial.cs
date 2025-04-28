
using Shared.Models.Courses;

namespace Shared.Models.Tests;
public class TestMaterial
{
    public int TestId { get; set; }
    public Test Test { get; set; } = default!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = default!;
}