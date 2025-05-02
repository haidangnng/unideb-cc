using Shared.Models.Courses;

namespace Shared.DTOs.Tests;
public class TestResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public bool ShuffleQuestions { get; set; }
    public bool AllowMultipleAttempts { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TestQuestionResponseDTO> Questions { get; set; } = new();
}

public class TestQuestionResponseDTO
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Choices { get; set; } = new();
    public string CorrectAnswer { get; set; } = string.Empty;
}

public class TestDetailDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int? TimeLimitMinutes { get; set; }
    public bool ShuffleQuestions { get; set; }
    public bool AllowMultipleAttempts { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<TestQuestionDTO> Questions { get; set; } = new();
    public List<Material> Materials { get; set; } = new();
}

public class TestQuestionDTO
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Choices { get; set; } = new();
    public string CorrectAnswer { get; set; } = string.Empty;
}