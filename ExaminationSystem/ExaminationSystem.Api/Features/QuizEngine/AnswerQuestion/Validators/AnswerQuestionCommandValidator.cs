using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Commands;
using FluentValidation;

namespace ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Validators
{
    public class AnswerQuestionCommandValidator : AbstractValidator<AnswerQuestionCommand>
    {
        public AnswerQuestionCommandValidator()
        {
            RuleFor(x => x.AttemptId).NotEmpty().WithMessage("Attempt ID is required.");
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("Student ID is required.");
            RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Question ID is required.");
            RuleFor(x => x.SelectedOptionId).NotEmpty().WithMessage("Selected option ID is required.");
        }
    }
}
