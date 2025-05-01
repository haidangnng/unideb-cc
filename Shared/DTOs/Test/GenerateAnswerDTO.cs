
namespace Shared.DTOs.Tests;
public class GenerateAnswerDTO
{
    public string QuestionText { get; set; }
    public List<int> MaterialIds { get; set; } = new();
}