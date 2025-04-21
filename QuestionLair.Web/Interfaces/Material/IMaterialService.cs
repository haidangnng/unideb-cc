using Microsoft.AspNetCore.Components.Forms;
using Shared.Models.Courses;

namespace QuestionLair.Web.Interfaces.Materials;

public interface IMaterialService
{
    Task<HttpResponseMessage> UploadMaterialAsync(int courseId, List<IBrowserFile> files);
    Task<HttpResponseMessage> UploadMaterialRawAsync(MultipartFormDataContent content);
    Task<List<Material>> GetMaterialsByCourseId(int courseId);
}
