namespace Shared.DTOs.Tests;

public class CreateTestQuestionDTO
{
    public string Question { get; set; } = string.Empty;

    public List<string> Choices { get; set; } = new();

    public string CorrectAnswer { get; set; } = string.Empty;
}
