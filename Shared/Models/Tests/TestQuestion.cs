using System.Text.Json;

namespace Shared.Models.Tests;

public class TestQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;

    // Store choices in JSON format (can be dynamic length: 1, 2, 3, 4... choices)
    public string? ChoicesJson { get; set; }
    public string AnswerText { get; set; } = string.Empty;
    public bool IsMultipleAnswer { get; set; } = false;
    public int TestId { get; set; }
    public Test Test { get; set; } = default!;
    public List<string> GetChoices()
    {
        if (string.IsNullOrEmpty(ChoicesJson))
            return new List<string>();
        return JsonSerializer.Deserialize<List<string>>(ChoicesJson) ?? new List<string>();
    }

    public void SetChoices(List<string> choices)
    {
        ChoicesJson = JsonSerializer.Serialize(choices);
    }
}