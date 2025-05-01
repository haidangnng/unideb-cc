using Shared.DTOs.Tests;
using Shared.Models.Tests;

public interface ITestService
{
    Task<Test?> CreateTestAsync(CreateTestDTO dto);
    Task<string> GenerateQuestionAsync(GenerateQuestionDTO dto);
    Task<string> GenerateAnswerAsync(GenerateAnswerDTO dto);
}