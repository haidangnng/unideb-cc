
using Shared.Models.Courses;

namespace QuestionLair.API.Interfaces;

public interface IMaterialService
{
    Task<List<Material>> CreateMaterial(int courseId, List<string> filePaths, List<string> fileNames, int teacherUserId);
    Task<List<Material>> GetMaterialsByCourseId(int courseId);
}
