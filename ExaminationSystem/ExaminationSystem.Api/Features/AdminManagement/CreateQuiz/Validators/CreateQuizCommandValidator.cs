using ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.Command;
using FluentValidation;

namespace ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.Validators
{
    public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
    {
        public CreateQuizCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.");

            RuleFor(x => x.PassScore)
                .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");

            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("Diploma ID is required.");
        }
    }
}