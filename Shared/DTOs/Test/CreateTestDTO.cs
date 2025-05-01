namespace Shared.DTOs.Tests;
public class CreateTestDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<int> SelectedMaterialIds { get; set; } = new();

    public bool ShuffleQuestions { get; set; } = false;
    public bool AllowMultipleAttempts { get; set; } = false;
    public int? TimeLimitMinutes { get; set; }

    public int NumberOfQuestions { get; set; } = 10;

    public List<CreateTestQuestionDTO> Questions { get; set; } = new();
}