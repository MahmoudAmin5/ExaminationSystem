using FluentValidation;

namespace ExaminationSystem.Api.Features.Auth.Register
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().Length(2, 100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
                .Matches(@"[A-Z]").Matches(@"[^a-zA-Z0-9]");
        }
    }
}
