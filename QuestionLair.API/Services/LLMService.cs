using System.Text.Json;
using QuestionLair.API.DTOs;

namespace QuestionLair.API.Services;
public class LLMService
{
    private readonly HttpClient _httpClient;

    public LLMService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5500");
    }

    public async Task CreateLLMFile(List<FileMetadataDto> dto)
    {
        var response = await _httpClient.PostAsJsonAsync("files/process", dto);
        response.EnsureSuccessStatusCode();
    }
}