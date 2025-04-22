using Microsoft.EntityFrameworkCore;
using QuestionLair.API.Data;
using QuestionLair.API.Interfaces;
using Shared.Models.Courses;

namespace QuestionLair.API.Services;
public class MaterialService : IMaterialService
{
    private readonly AppDbContext _context;

    public MaterialService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Material>> CreateMaterial(int courseId, List<string> filePaths, List<string> fileNames, int teacherUserId)
    {
        var materials = new List<Material>();

        for (int i = 0; i < filePaths.Count; i++)
        {
            var material = new Material
            {
                CourseId = courseId,
                Url = filePaths[i],
                FileName = fileNames[i],
                UploadedBy = teacherUserId,
            };

            materials.Add(material);
        }

        await _context.Materials.AddRangeAsync(materials);
        await _context.SaveChangesAsync();

        return materials;
    }
    public async Task<List<Material>> GetMaterialsByCourseId(int courseId)
    {
        return await _context.Materials
            .Where(m => m.CourseId == courseId)
            .ToListAsync();
    }
}
