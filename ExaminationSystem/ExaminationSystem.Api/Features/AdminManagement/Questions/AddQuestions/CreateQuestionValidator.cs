using FluentValidation;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions
{
    public class AddQuestionOrchestratorValidator : AbstractValidator<AddQuestionOrchestrator>
    {
        public AddQuestionOrchestratorValidator()
        {
            RuleFor(x => x.Text)
           .NotEmpty().WithMessage("Question text is required.");

            RuleFor(x => x.Options)
                .NotNull()
                .Must(x => x.Count >= 2).WithMessage("At least 2 options required.")
                .Must(x => x != null && x.Count(o => o.IsCorrect) == 1).WithMessage("Exactly one correct option required.");
        }
    }
}
