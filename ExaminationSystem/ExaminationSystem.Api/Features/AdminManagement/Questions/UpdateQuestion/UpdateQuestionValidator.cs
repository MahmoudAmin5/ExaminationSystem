using FluentValidation;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionOrchestrator>
    {
        public UpdateQuestionValidator()
        {
            RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text cannot be empty.");

             RuleFor(x => x.Options)
            .NotNull().WithMessage("Options list is required.")
            .Must(x => x != null && x.Count >= 2)
            .WithMessage("At least 2 options are required for each question.")
            .Must(x => x != null && x.Count(o => o.IsCorrect) == 1)
            .WithMessage("Exactly one option must be marked as correct.");
    }
}
}
