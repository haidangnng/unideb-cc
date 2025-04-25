using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Tests;
using System.Net.Http.Json;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Teacher")]
public class TestsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TestsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("generate-ai-test")]
    public async Task<IActionResult> GenerateAiTest([FromBody] GenerateTestRequestDto dto)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.PostAsJsonAsync("http://localhost:8000/ai/generate_test", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, error);
        }

        var questions = await response.Content.ReadFromJsonAsync<List<GeneratedQuestionDto>>();

        return Ok(questions);
    }
}
