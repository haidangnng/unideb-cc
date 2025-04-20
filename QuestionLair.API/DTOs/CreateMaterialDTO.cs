namespace QuestionLair.API.DTOs;

public class CreateMaterialDto
{
    public int CourseId { get; set; }

    public List<IFormFile> Files { get; set; } = [];
}
