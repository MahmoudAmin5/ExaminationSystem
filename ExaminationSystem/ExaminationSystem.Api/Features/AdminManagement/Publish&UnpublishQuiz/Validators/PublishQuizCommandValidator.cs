using ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Command;
using FluentValidation;

namespace ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Validators
{
    public class PublishQuizCommandValidator : AbstractValidator<PublishQuizCommand>
    {
        public PublishQuizCommandValidator()
        {
            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("Quiz ID is required.");
        }
    }

}
