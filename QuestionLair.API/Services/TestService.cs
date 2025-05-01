using Microsoft.EntityFrameworkCore;
using QuestionLair.API.Data;
using QuestionLair.API.Interfaces;
using Shared.DTOs.Tests;
using Shared.Models.Tests;
using Shared.Models.Courses;
using System.Text.Json;

namespace QuestionLair.API.Services;

public class TestService : ITestService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public TestService(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Test> CreateTestAsync(CreateTestDTO dto, int teacherUserId)
    {
        var test = new Test
        {
            Title = dto.Title,
            Description = dto.Description,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            ShuffleQuestions = dto.ShuffleQuestions,
            AllowMultipleAttempts = dto.AllowMultipleAttempts,
            CreatedBy = teacherUserId,
            Questions = dto.Questions.Select(q => new TestQuestion
            {
                QuestionText = q.Question,
                ChoicesJson = JsonSerializer.Serialize(q.Choices),
                CorrectAnswer = q.CorrectAnswer ?? string.Empty,
            }).ToList()
        };

        _context.Tests.Add(test);
        await _context.SaveChangesAsync();
        return test;
    }

    // public async Task<List<CreateTestQuestionDTO>> GenerateQuestionsFromMaterials(List<int> materialIds, int count)
    // {
    //     var client = _httpClientFactory.CreateClient("LLM");
    //     var response = await client.PostAsJsonAsync("files/generate-questions", new { material_ids = materialIds, count });
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadFromJsonAsync<List<CreateTestQuestionDTO>>() ?? new();
    // }

    // public async Task<string> GenerateAnswerForQuestion(GenerateAnswerDTO dto)
    // {
    //     var client = _httpClientFactory.CreateClient("LLM");
    //     var response = await client.PostAsJsonAsync("files/generate-answer", new { question = dto.QuestionText, materials = dto.MaterialIds });
    //     response.EnsureSuccessStatusCode();
    //     return await response.Content.ReadAsStringAsync();
    // }
}
