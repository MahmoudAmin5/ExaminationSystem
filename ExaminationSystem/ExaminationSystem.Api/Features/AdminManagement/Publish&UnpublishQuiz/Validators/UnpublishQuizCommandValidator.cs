using ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Command;
using FluentValidation;

namespace ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Validators
{
    public class UnpublishQuizCommandValidator : AbstractValidator<UnpublishQuizCommand>
    {
        public UnpublishQuizCommandValidator()
        {
            RuleFor(x => x.quizId)
                .NotEmpty().WithMessage("Quiz ID is required.");
        }
    }
}
