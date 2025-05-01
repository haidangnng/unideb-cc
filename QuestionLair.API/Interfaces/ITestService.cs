using Shared.DTOs.Tests;
using Shared.Models.Tests;
using System.Text.Json;

namespace QuestionLair.API.Interfaces;

public interface ITestService
{
    Task<Test> CreateTestAsync(CreateTestDTO dto, int teacherUserId);
    // Task<List<CreateTestQuestionDTO>> GenerateQuestionsFromMaterials(List<int> materialIds, int count);
    // Task<string> GenerateAnswerForQuestion(GenerateAnswerDTO dto);
}
