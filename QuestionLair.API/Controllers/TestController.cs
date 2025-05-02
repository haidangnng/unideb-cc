using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionLair.API.Data;
using QuestionLair.API.Interfaces;
using Shared.DTOs.Tests;
using System.Net.Http.Json;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Teacher")]
public class TestsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITestService _testService;

    public TestsController(AppDbContext context, ITestService testService)
    {

        _context = context;
        _testService = testService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestDTO dto)
    {
        var teacherUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var test = await _testService.CreateTestAsync(dto, teacherUserId);
        return Ok(test);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TestDetailDTO>> GetTestById(int id)
    {
        var test = await _testService.GetTestByIdAsync(id);
        if (test == null)
            return NotFound();

        return Ok(test);
    }

    [HttpGet("course/{courseId}")]
    [Authorize]
    public async Task<IActionResult> GetTestsByCourseId(int courseId)
    {
        try
        {
            var tests = await _testService.GetTestByCourseId(courseId);
            return Ok(tests); // safe to return DTOs
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRRRRRR ====> {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }
}
