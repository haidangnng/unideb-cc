using QuestionLair.API.Data;
using QuestionLair.API.Interfaces;
using System.Security.Claims;

namespace QuestionLair.API.Controllers;

using QuestionLair.API.DTOs;
using QuestionLair.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Courses;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class MaterialController(
  AppDbContext context,
  IMaterialService materialService,
  S3Service s3Service,
  LLMService llmService) : ControllerBase
{
    private readonly LLMService _llmService = llmService;

    private readonly AppDbContext _context = context;
    private readonly IMaterialService _materialService = materialService;
    private readonly S3Service _s3Service = s3Service;

    private int GetUserId() =>
      int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ========================
    // Teacher: Create material
    // ========================
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateMaterial([FromForm] CreateMaterialDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        List<IFormFile> files = dto.Files;
        int courseId = dto.CourseId;

        try
        {
            if (files.Count == 0)
                return BadRequest("File is required.");

            // Validate file types
            // var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".xlsx", ".xls", ".doc", ".docx", ".pdf", ".ppt", ".pptx" };
            var allowedExtensions = new[] { ".pdf" };
            var invalidFiles = files.Where(f => !allowedExtensions.Contains(Path.GetExtension(f.FileName).ToLowerInvariant())).ToList();

            if (invalidFiles.Any())
            {
                return BadRequest($"Invalid file type(s): {string.Join(", ", invalidFiles.Select(f => Path.GetFileName(f.FileName)))}. " +
                                  "Only images, Excel, Word, PDF, and PowerPoint files are allowed.");
            }

            List<string> filePaths = new List<string>();
            List<string> fileNames = new List<string>();

            Console.WriteLine("==== file length ====", files.Count);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    string filePath = await _s3Service.UploadAsync(file);
                    Console.WriteLine($"in loop {filePath}");

                    filePaths.Add(filePath);
                    fileNames.Add(file.FileName);
                }
            }

            Console.WriteLine($"fileNames COunt: {fileNames.Count}");
            Console.WriteLine($"filePaths Count: {filePaths.Count}");

            List<Material> materials = await _materialService.CreateMaterial(courseId, filePaths, fileNames, GetUserId());

            List<FileMetadataDto> llmMaterials = new List<FileMetadataDto>(); ;
            foreach (var material in materials)
            {

                var metadata = new FileMetadataDto
                {
                    url = material.Url,
                    id = material.Id
                };
                llmMaterials.Add(metadata);
            }

            _ = _llmService.CreateLLMFile(llmMaterials);

            await transaction.CommitAsync();

            return Ok(new { message = "Files uploaded successfully", materials = materials });
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();

            return BadRequest(e.Message);
        }
    }

    [HttpGet("course/{courseId}")]
    [Authorize]
    public async Task<IActionResult> GetMaterialsByCourseId(int courseId)
    {
        try
        {
            var materials = await _materialService.GetMaterialsByCourseId(courseId);
            return Ok(materials);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}