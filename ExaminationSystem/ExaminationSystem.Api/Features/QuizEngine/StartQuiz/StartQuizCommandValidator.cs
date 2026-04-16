using FluentValidation;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz
{
    public class StartQuizCommandValidator : AbstractValidator<StartQuizCommand>
    {
        public StartQuizCommandValidator()
        {
            RuleFor(x => x.QuizId)
                .NotEmpty()
                .WithMessage("Quiz ID is required.");

            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Student ID is required.");
        }
    }
}
