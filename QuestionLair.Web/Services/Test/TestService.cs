using Shared.DTOs.Tests;
using Shared.Models.Tests;

public class TestService : ITestService
{
    private readonly HttpClient _http;
    private readonly HttpClient _httpLLM;

    public TestService(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient("ApiClient");
        _httpLLM = clientFactory.CreateClient("ApiLLM");
    }

    public async Task<Test?> CreateTestAsync(CreateTestDTO dto)
    {
        var response = await _http.PostAsJsonAsync("api/tests", dto);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<Test>();
    }

    public async Task<string> GenerateQuestionAsync(GenerateQuestionDTO dto)
    {
        var response = await _httpLLM.PostAsJsonAsync("generate-question", dto);
        if (!response.IsSuccessStatusCode) return "Failed to generate question.";

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GenerateAnswerAsync(GenerateAnswerDTO dto)
    {
        var response = await _httpLLM.PostAsJsonAsync("generate-answer", dto);
        if (!response.IsSuccessStatusCode) return "Failed to generate answer.";

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<TestDetailDTO>> GetTestByCourseId(int courseId)
    {
        var response = await _http.GetAsync($"api/tests/course/{courseId}");
        response.EnsureSuccessStatusCode();


        var tests = await response.Content.ReadFromJsonAsync<List<TestDetailDTO>>();
        return tests ?? new List<TestDetailDTO>();
    }

    public async Task<TestDetailDTO?> GetTestByIdAsync(int testId)
    {
        var response = await _http.GetAsync($"api/tests/{testId}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<TestDetailDTO>();
    }
}