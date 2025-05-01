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
}