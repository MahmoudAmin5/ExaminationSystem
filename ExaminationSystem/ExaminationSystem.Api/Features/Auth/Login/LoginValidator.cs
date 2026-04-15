using FluentValidation;
using static ExaminationSystem.Api.Features.Auth.Login.Login;

namespace ExaminationSystem.Api.Features.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
