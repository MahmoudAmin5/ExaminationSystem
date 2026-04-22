using FluentValidation;
namespace ExaminationSystem.Api.Features.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginOrchestrator>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
