using QuestionLair.Web.Interfaces.Courses;
using Shared.DTOs.Courses;
using Shared.Models.Courses;
using System.Net.Http.Json;

namespace QuestionLair.Web.Services.Courses;

public class TestService : ITestService
{
    private readonly HttpClient _http;

    public TestService(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient("ApiClient");
    }

    // public async Task<TeacherCourseDTO?> CreateCourseAsync(CourseCreateDto dto)
    // {
    //     var response = await _http.PostAsJsonAsync("api/courses", dto);
    //     if (!response.IsSuccessStatusCode) return null;

    //     return await response.Content.ReadFromJsonAsync<TeacherCourseDTO>();
    // }
}