using Shared.Enums;

namespace Shared.DTOs.Tests;

public class GenerateTestRequestDto
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public List<int> MaterialIds { get; set; } = new();
    public int NumberOfQuestions { get; set; } = 5;
    public TestDifficulty Difficulty { get; set; } // Optional: "Easy", "Medium", "Hard"
    public string? Topic { get; set; } // Optional
}
