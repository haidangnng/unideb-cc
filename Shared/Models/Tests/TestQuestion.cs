using System.Text.Json;

namespace Shared.Models.Tests;

public class TestQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;

    public string? ChoicesJson { get; set; } // Store multiple choices in JSON
    public string CorrectAnswer { get; set; } = string.Empty; // Only one correct answer

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