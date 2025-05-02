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

    public async Task<TestDetailDTO> CreateTestAsync(CreateTestDTO dto, int teacherUserId)
    {

        Console.WriteLine(JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true }));
        var course = await _context.Courses.FindAsync(dto.CourseId);
        if (course == null)
            throw new Exception("Course not found.");

        var test = new Test
        {
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            CourseId = course.Id,
            CreatedBy = teacherUserId,
            ShuffleQuestions = dto.ShuffleQuestions,
            AllowMultipleAttempts = dto.AllowMultipleAttempts,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            Duration = dto.TimeLimitMinutes ?? 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tests.Add(test);
        await _context.SaveChangesAsync();

        _context.TestMaterials.AddRange(dto.SelectedMaterialIds.Select(id => new TestMaterial
        {
            TestId = test.Id,
            MaterialId = id
        }));

        _context.TestQuestions.AddRange(dto.Questions.Select(q => new TestQuestion
        {
            QuestionText = q.Question,
            ChoicesJson = JsonSerializer.Serialize(q.Choices),
            CorrectAnswer = q.CorrectAnswer,
            TestId = test.Id
        }));

        await _context.SaveChangesAsync();

        // Reload with related data
        var fullTest = await _context.Tests
            .Include(t => t.Questions)
            .Include(t => t.TestMaterials).ThenInclude(tm => tm.Material)
            .FirstOrDefaultAsync(t => t.Id == test.Id);

        return new TestDetailDTO
        {
            Id = fullTest.Id,
            Title = fullTest.Title,
            Description = fullTest.Description,
            TimeLimitMinutes = fullTest.TimeLimitMinutes,
            ShuffleQuestions = fullTest.ShuffleQuestions,
            AllowMultipleAttempts = fullTest.AllowMultipleAttempts,
            CreatedAt = fullTest.CreatedAt,
            UpdatedAt = fullTest.UpdatedAt,
            Questions = fullTest.Questions.Select(q => new TestQuestionDTO
            {
                QuestionText = q.QuestionText,
                Choices = q.GetChoices(),
                CorrectAnswer = q.CorrectAnswer
            }).ToList(),
            Materials = fullTest.TestMaterials.Select(tm => new Material
            {
                Id = tm.Material.Id,
                FileName = tm.Material.FileName,
                Url = tm.Material.Url
            }).ToList()
        };
    }

    public async Task<TestDetailDTO?> GetTestByIdAsync(int testId)
    {
        var test = await _context.Tests
            .Include(t => t.Questions)
            .Include(t => t.TestMaterials)
                .ThenInclude(tm => tm.Material)
            .FirstOrDefaultAsync(t => t.Id == testId);

        if (test == null)
            return null;

        return new TestDetailDTO
        {
            Id = test.Id,
            Title = test.Title,
            Description = test.Description,
            TimeLimitMinutes = test.TimeLimitMinutes,
            ShuffleQuestions = test.ShuffleQuestions,
            AllowMultipleAttempts = test.AllowMultipleAttempts,
            CreatedAt = test.CreatedAt,
            UpdatedAt = test.UpdatedAt,
            Questions = test.Questions.Select(q => new TestQuestionDTO
            {
                QuestionText = q.QuestionText,
                Choices = q.GetChoices(),
                CorrectAnswer = q.CorrectAnswer
            }).ToList(),
            Materials = test.TestMaterials.Select(tm => new Material
            {
                Id = tm.Material.Id,
                FileName = tm.Material.FileName,
                Url = tm.Material.Url
            }).ToList()
        };
    }
    public async Task<List<TestDetailDTO>> GetTestByCourseId(int courseId)
    {
        var tests = await _context.Tests
               .Include(t => t.Questions)
               .Include(t => t.TestMaterials)
                   .ThenInclude(tm => tm.Material)
               .Where(t => t.CourseId == courseId)
               .ToListAsync();

        return tests.Select(test => new TestDetailDTO
        {
            Id = test.Id,
            Title = test.Title,
            Description = test.Description,
            TimeLimitMinutes = test.TimeLimitMinutes,
            ShuffleQuestions = test.ShuffleQuestions,
            AllowMultipleAttempts = test.AllowMultipleAttempts,
            CreatedAt = test.CreatedAt,
            UpdatedAt = test.UpdatedAt,
            Questions = test.Questions.Select(q => new TestQuestionDTO
            {
                QuestionText = q.QuestionText,
                Choices = q.GetChoices(),
                CorrectAnswer = q.CorrectAnswer
            }).ToList(),
            Materials = test.TestMaterials.Select(tm => new Material
            {
                Id = tm.Material.Id,
                FileName = tm.Material.FileName,
                Url = tm.Material.Url
            }).ToList()
        }).ToList();
    }
}
