using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionLair.API.Interfaces;
using Shared.DTOs.Tests;
using System.Net.Http.Json;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Teacher")]
public class TestsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITestService _testService;

    public TestsController(IHttpClientFactory httpClientFactory
        , ITestService testService)
    {
        _httpClientFactory = httpClientFactory;
        _testService = testService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestDTO dto)
    {
        var teacherUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var test = await _testService.CreateTestAsync(dto, teacherUserId);
        return Ok(test);
    }

    // [HttpPost("generate-questions")]
    // public async Task<IActionResult> GenerateQuestions([FromBody] GenerateQuestionDTO dto)
    // {
    //     var questions = await _testService.GenerateQuestionsFromMaterials(dto.MaterialIds, 1);
    //     return Ok(questions);
    // }

    // [HttpPost("generate-answer")]
    // public async Task<IActionResult> GenerateAnswer([FromBody] GenerateAnswerDTO dto)
    // {
    //     var answer = await _testService.GenerateAnswerForQuestion(dto);
    //     return Ok(answer);
    // }
}
