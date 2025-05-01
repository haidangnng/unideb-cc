
namespace Shared.DTOs.Tests;
public class GenerateAnswerDTO
{
    public string Question { get; set; }
    public List<int> MaterialIds { get; set; } = new();
}