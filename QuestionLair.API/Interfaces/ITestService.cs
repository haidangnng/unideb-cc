using Shared.DTOs.Tests;
using Shared.Models.Tests;

namespace QuestionLair.API.Interfaces;

public interface ITestService
{
    Task<TestDetailDTO> CreateTestAsync(CreateTestDTO dto, int teacherUserId);
    Task<TestDetailDTO?> GetTestByIdAsync(int testId);
    Task<List<TestDetailDTO>> GetTestByCourseId(int courseId);
}