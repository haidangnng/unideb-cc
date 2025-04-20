using QuestionLair.Web.Interfaces.Courses;
using Shared.DTOs.Courses;
using Shared.Models.Courses;
using System.Net.Http.Json;

namespace QuestionLair.Web.Services.Courses;

public class CourseClientService : ICourseClientService
{
    private readonly HttpClient _http;

    public CourseClientService(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient("ApiClient");
    }

    public async Task<TeacherCourseDTO?> CreateCourseAsync(CourseCreateDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/courses", dto);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<TeacherCourseDTO>();
    }

    public async Task EnrollStudentAsync(int courseId)
    {
        var response = await _http.PostAsync($"api/courses/{courseId}/enroll", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Course>> GetMyCoursesAsync()
    {
        var response = await _http.GetAsync("api/courses/my");
        response.EnsureSuccessStatusCode();

        var courses = await response.Content.ReadFromJsonAsync<List<Course>>();
        return courses ?? new List<Course>();
    }
}
