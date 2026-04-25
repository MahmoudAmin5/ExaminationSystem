using ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Command;
using FluentValidation;

namespace ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Validators
{
    public class UpdateQuizCommandValidator : AbstractValidator<UpdateQuizCommand>
    {
        public UpdateQuizCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Quiz ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("Diploma ID is required.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.");

            RuleFor(x => x.PassScore)
                .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");

            RuleFor(x => x.MaxAttempts)
                .GreaterThan(0).WithMessage("Max attempts must be at least 1.");

            RuleFor(x => x.Instructions)
                .NotEmpty().WithMessage("Instructions are required.");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
