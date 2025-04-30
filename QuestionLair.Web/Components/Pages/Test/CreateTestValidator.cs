using FluentValidation;
using Shared.DTOs.Tests;

public class CreateTestFluentValidator : AbstractValidator<CreateTestDTO>
{
    public CreateTestFluentValidator()
    {
        RuleFor(x => x.Title)
                  .NotEmpty().WithMessage("Test title is required.")
                  .MaximumLength(100).WithMessage("Test title must be less than 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        // Material selection
        RuleFor(x => x.SelectedMaterialIds)
            .NotEmpty().WithMessage("Please select at least one material.");

        // Number of Questions
        RuleFor(x => x.NumberOfQuestions)
            .GreaterThan(0).WithMessage("Number of questions must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Number of questions must be less than or equal to 100.");

        // Settings Validation
        RuleFor(x => x.TimeLimitMinutes)
            .GreaterThan(0)
            .When(x => x.TimeLimitMinutes.HasValue)
            .WithMessage("Time limit must be greater than 0 if set.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CreateTestDTO>.CreateWithOptions((CreateTestDTO)model, x =>
x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}

public class CreateTestQuestionFluentValidator : AbstractValidator<CreateTestQuestionDTO>
{
    public CreateTestQuestionFluentValidator()
    {
        RuleFor(q => q.Question)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(500).WithMessage("Question must be less than 500 characters.");

        RuleFor(q => q.Choices)
            .NotNull().WithMessage("Choices are required.")
            .Must(c => c.Count >= 2 && c.Count <= 6)
            .WithMessage("You must provide between 2 and 6 choices.");

        RuleForEach(q => q.Choices)
            .NotEmpty().WithMessage("Each choice must have some text.")
            .MaximumLength(200).WithMessage("Each choice must be less than 200 characters.");

        RuleFor(q => q.CorrectAnswer)
            .NotEmpty().WithMessage("A correct answer is required.")
            .Must((q, correctAnswer) => q.Choices.Contains(correctAnswer))
            .WithMessage("Correct answer must be one of the provided choices.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CreateTestQuestionDTO>.CreateWithOptions((CreateTestQuestionDTO)model, x =>
            x.IncludeProperties(propertyName)));

        if (result.IsValid)
            return Array.Empty<string>();

        return result.Errors.Select(e => e.ErrorMessage);
    };
}