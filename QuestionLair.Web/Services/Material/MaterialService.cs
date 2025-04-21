using Microsoft.AspNetCore.Components.Forms;
using QuestionLair.Web.Interfaces.Materials;
using Shared.Models.Courses;
using System.Net.Http.Headers;

namespace QuestionLair.Web.Services.Materials;

public class MaterialService : IMaterialService
{
    private readonly HttpClient _http;

    public MaterialService(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient("ApiClient");
    }

    public async Task<HttpResponseMessage> UploadMaterialAsync(int courseId, List<IBrowserFile> files)
    {
        using var content = new MultipartFormDataContent();

        foreach (var file in files)
        {
            var stream = file.OpenReadStream(maxAllowedSize: 10_000_000); // 10MB max
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "Files", file.Name);
        }

        content.Add(new StringContent(courseId.ToString()), "CourseId");

        var response = await _http.PostAsync("api/material", content);
        return response;
    }
    public Task<HttpResponseMessage> UploadMaterialRawAsync(MultipartFormDataContent content)
    {
        return _http.PostAsync("api/material", content);
    }

    public async Task<List<Material>> GetMaterialsByCourseId(int id)
    {
        var response = await _http.GetAsync($"api/material/course/{id}");
        response.EnsureSuccessStatusCode();

        var materials = await response.Content.ReadFromJsonAsync<List<Material>>();
        return materials ?? new List<Material>();
    }
}
